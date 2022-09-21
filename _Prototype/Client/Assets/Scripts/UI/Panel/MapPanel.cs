using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : Panel
{
    public static MapPanel Instance { get; private set; }

    [SerializeField]
    private SerializableDictionary<Area, MapAreaInfoUI> mapAreaInfoDic = new SerializableDictionary<Area, MapAreaInfoUI>();

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        MapAreaInfoUI[] uis = GetComponentsInChildren<MapAreaInfoUI>();

        for (int i = 0; i < uis.Length; i++)
        {
            mapAreaInfoDic.Add(uis[i].Area, uis[i]);
        }
    }

    public void InitMapAreaInfoUI()
    {
        foreach (var item in mapAreaInfoDic)
        {
            item.Value.Init();
        }
    }

    public MapAreaInfoUI GetMapAreaInfoUI(Area area)
    {
        MapAreaInfoUI mapAreaInfoUI = null;

        if(mapAreaInfoDic.ContainsKey(area))
        { 
            mapAreaInfoUI = mapAreaInfoDic[area];
        }

        if(mapAreaInfoUI == null)
        {
            mapAreaInfoUI = mapAreaInfoDic[Area.Altar];
        }

        return mapAreaInfoUI;
    }
}
