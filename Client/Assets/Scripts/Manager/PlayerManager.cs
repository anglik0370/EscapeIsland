using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    private Transform indoorColliderParentTrm;
    private List<Collider2D> indoorColList = new List<Collider2D>();

    private const float STATE_UPDATE_DELAY = 0.1f;

    private Coroutine co;

    public List<Player> PlayerList => NetworkManager.instance.GetPlayerList();

    private Player player;
    public Player Player => player;

    [SerializeField]
    private Inventory inventory;

    private Kill kill;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        indoorColList = indoorColliderParentTrm.GetComponentsInChildren<Collider2D>().ToList();
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
            player.inventory = inventory;

            if(co != null)
            {
                StopCoroutine(co);
            }

            co = StartCoroutine(UpdatePlayerAreaStateRoutine());
        });

        EventManager.SubGameStart(p =>
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if(p.isKidnapper && !PlayerList[i].isKidnapper)
                    GameManager.Instance.AddInteractionObj(PlayerList[i]);
            }
        });

        EventManager.SubExitRoom(() =>
        {
            if (co != null)
            {
                StopCoroutine(co);
            }
        });

        EventManager.SubGameOver(goc =>
        {
            if (co != null)
            {
                StopCoroutine(co);
            }

            co = StartCoroutine(UpdatePlayerAreaStateRoutine());
        });

        StartCoroutine(CoroutineHandler.Frame(() => kill = NetworkManager.instance.FindSetDataScript<Kill>()));
    }

    public void KillPlayer(Player p)
    {
        TimeHandler.Instance.InitKillCool();
        kill.KillPlayer(p);
    }

    public bool AmIKidnapper()
    {
        return player.isKidnapper;
    }

    public bool AmIDead()
    {
        return player.isDie;
    }

    public bool AmIMaster()
    {
        return player.master;
    }

    private IEnumerator UpdatePlayerAreaStateRoutine()
    {
        //?????? ???????? ?????? ???? ?????? ????
        WaitForSeconds delay = new WaitForSeconds(STATE_UPDATE_DELAY);
        bool isTouching;

        while (true)
        {
            isTouching = false;

            for (int j = 0; j < indoorColList.Count; j++)
            {
                if (Physics2D.IsTouching(indoorColList[j], player.FootCollider))
                {
                    player.SetAreaState((AreaState)j + 1);
                    isTouching = true;
                    break;
                }
            }

            if (!isTouching)
            {
                player.SetAreaState(AreaState.OutSide);
            }

            SendManager.Instance.SendAreaState(player.AreaState);
            yield return delay;
        }
    }
}
