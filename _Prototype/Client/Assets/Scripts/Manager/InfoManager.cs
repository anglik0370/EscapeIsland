using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;

    private List<InfoUI> infoList = new List<InfoUI>();
    private List<TeamInfoUI> teamInfoList = new List<TeamInfoUI>();

    public Transform parentTrm;
    public GameObject infoPrefab;
    public GameObject teamInfoPrefab;

    private Player mainPlayer;
    public Player MainPlayer
    {
        get => instance.mainPlayer;
        set => instance.mainPlayer = value;
    }

    private void Awake()
    {
        instance = this;
        PoolManager.CreatePool<InfoUI>(infoPrefab, parentTrm, 8);
        PoolManager.CreatePool<TeamInfoUI>(teamInfoPrefab, parentTrm, 8);
    }

    public static InfoUI SetInfoUI(Transform player, string name)
    {
        InfoUI ui = PoolManager.GetItem<InfoUI>();
        ui.SetTarget(player, instance.MainPlayer, name);
        if(!instance.infoList.Contains(ui))
        {
            instance.infoList.Add(ui);
        }
        return ui;
    }

    public static TeamInfoUI SetTeamInfoUI(Player p, string name)
    {
        TeamInfoUI ui = PoolManager.GetItem<TeamInfoUI>();
        ui.SetUser(name, p);

        if(!instance.teamInfoList.Contains(ui))
        {
            instance.teamInfoList.Add(ui);
        }

        return ui;
    }

    public static void InitTeamInfoUIs()
    {
        for (int i = 0; i < instance.teamInfoList.Count; i++)
        {
            instance.teamInfoList[i].SetActive(false);
        }
        instance.teamInfoList.Clear();
    }

    public static TeamInfoUI GetTeamInfoUI(int socId)
    {
        return instance.teamInfoList.Find(x => x.SocketId.Equals(socId));
    }

}
