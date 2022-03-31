using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;

    private List<InfoUI> infoList = new List<InfoUI>();

    public Transform parentTrm;
    public GameObject infoPrefab;

    private Transform mainPlayerTrm;
    private static Transform MainPlayerTrm => instance.mainPlayerTrm;

    private void Awake()
    {
        instance = this;
        PoolManager.CreatePool<InfoUI>(infoPrefab, parentTrm, 8);
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            mainPlayerTrm = p.transform;
            infoList.ForEach(info => info.SetTarget(mainPlayerTrm));
        });
    }

    public static InfoUI SetInfoUI(Transform player, string name)
    {
        InfoUI ui = PoolManager.GetItem<InfoUI>();
        ui.SetTarget(player, MainPlayerTrm, name);
        instance.infoList.Add(ui);
        return ui;
    }
}
