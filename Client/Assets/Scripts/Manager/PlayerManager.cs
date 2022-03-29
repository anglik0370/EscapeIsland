using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<Player> PlayerList => NetworkManager.instance.GetPlayerList();

    private Player player;
    public Player Player => player;

    private Kill kill;

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

            player.inventory = FindObjectOfType<Inventory>();
        });

        EventManager.SubGameStart(p =>
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                GameManager.Instance.AddInteractionObj(PlayerList[i]);
            }
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
}
