using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<Player> PlayerList => NetworkManager.instance.GetPlayerList();

    private Player player;
    public Player Player => player;

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

    public bool FindProximatePlayer(out Player temp)
    {
        Player p = null;

        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].isDie) continue;

            if (Vector2.Distance(player.GetTrm().position, PlayerList[i].transform.position) <= player.range)
            {
                if (p == null)
                {
                    p = PlayerList[i];
                }
                else
                {
                    if (Vector2.Distance(player.GetTrm().position, p.transform.position) >
                        Vector2.Distance(player.GetTrm().position, PlayerList[i].transform.position))
                    {
                        p = PlayerList[i];
                    }
                }
            }
        }

        temp = p;

        return temp != null;
    }

    public void KillProximatePlayer()
    {
        if (!TimeHandler.Instance.isKillAble)
        {
            //ų ������ �����մϴ� <- �޽��� ǥ��
            UIManager.Instance.SetWarningText("���� ų �� �� �����ϴ�.");
            return;
        }

        FindProximatePlayer(out Player targetPlayer);

        if (targetPlayer == null) return;

        targetPlayer.SetDead();

        TimeHandler.Instance.InitKillCool();

        NetworkManager.instance.Kill(targetPlayer);
    }
}
