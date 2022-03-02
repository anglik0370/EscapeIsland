using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;

    public GameObject infoPrefab;

    private void Awake()
    {
        instance = this;
        PoolManager.CreatePool<InfoUI>(infoPrefab, transform, 8);
    }
    public static InfoUI SetInfoUI(Transform player, string name)
    {
        InfoUI ui = PoolManager.GetItem<InfoUI>();
        ui.SetTarget(player, name);
        return ui;
    }
}