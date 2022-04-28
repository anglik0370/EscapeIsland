using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;

    private List<InfoUI> infoList = new List<InfoUI>();

    public Transform parentTrm;
    public GameObject infoPrefab;

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

    public static InfoUI FindInfoUI(int playerId)
    {
        return instance.infoList.Find(ui => ui.MainPlayer.socketId == playerId && ui.gameObject.activeSelf);
    }

}
