using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    private Transform areaColParent;
    [SerializeField]
    private List<AreaCollider> areaColList;

    private const float STATE_UPDATE_DELAY = 0.1f;

    private Coroutine co;

    public List<Player> PlayerList => NetworkManager.instance.GetPlayerList();

    private Player player;
    public Player Player => player;

    [SerializeField]
    private Inventory inventory;
    public Inventory Inventory => inventory;

    [SerializeField]
    private List<Area> areaList = new List<Area>();
    [SerializeField]
    private OccupyUI occupyUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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

        //EventManager.SubGameStart(p =>
        //{
        //    for (int i = 0; i < PlayerList.Count; i++)
        //    {
        //        if(p.isKidnapper && !PlayerList[i].isKidnapper)
        //            GameManager.Instance.AddInteractionObj(PlayerList[i]);
        //    }
        //});

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

        for (int i = 0; i < areaColParent.childCount; i++)
        {
            AreaCollider ac = new AreaCollider();

            ac.area = areaColParent.GetChild(i).GetComponent<AreaStateHolder>().Area;
            ac.collider = areaColParent.GetChild(i).GetComponent<Collider2D>();

            areaColList.Add(ac);
        }
    }

    public bool AmIMaster()
    {
        return player.master;
    }

    public int GetRangeInPlayerId(float range)
    {
        float minRange = float.MaxValue;
        float curRange = float.MaxValue;

        int targetSocId = 0;

        foreach (Player p in PlayerList)
        {
            if (p.CurTeam.Equals(player.CurTeam)) continue;

            curRange = Vector2.Distance(player.transform.position, p.transform.position);

            if (curRange <= range && curRange < minRange)
            {
                minRange = curRange;
                targetSocId = p.socketId;
            }
        }

        return targetSocId;
    }

    private IEnumerator UpdatePlayerAreaStateRoutine()
    {
        //딜레이 간격으로 자신의 위치 상태를 갱신
        WaitForSeconds delay = new WaitForSeconds(STATE_UPDATE_DELAY);
        bool isTouching;

        while (true)
        {
            isTouching = false;

            foreach (var ac in areaColList)
            {
                if(Physics2D.IsTouching(ac.collider, player.FootCollider))
                {
                    player.SetAreaState(ac.area);
                    isTouching = true;
                    break;
                }
            }

            if (!isTouching)
            {
                player.SetAreaState(Area.None);
            }

            if(areaList.Contains(player.Area))
            {
                occupyUI.EnableUI();
            }
            else
            {
                occupyUI.DisableUI();
            }

            SendManager.Instance.SendAreaState(player.Area);
            yield return delay;
        }
    }

    public void AccelerationPlayer(int id)
    {
        TimedBuff buff = BuffManager.Instance.GetBuffSO(id)?.InitializeBuff(player.gameObject);

        if(buff != null)
            player.BuffHandler.AddBuff(buff);
    }
}
