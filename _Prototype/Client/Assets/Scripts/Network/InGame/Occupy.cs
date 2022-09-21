using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupy : ISetAble
{
    public static Occupy Instance { get; private set; }

    [SerializeField]
    private SerializableDictionary<Area, SpriteOutline> areaOutlineDic = new SerializableDictionary<Area, SpriteOutline>();

    [SerializeField]
    private List<SpriteOutline> outlineList = new List<SpriteOutline>();

    private OccupyVO data;

    private bool needOccupyRefresh = false;

    public static void SetOccupyData(OccupyVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.data = vo;
            Instance.needOccupyRefresh = true;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        for (int i = 1; i < 5; i++)
        {
            areaOutlineDic.Add((Area)Enum.GetValues(typeof(Area)).GetValue(i),outlineList[i - 1]);
        }
    }

    private void Update()
    {
        if (needOccupyRefresh)
        {
            SetOccupy();
            needOccupyRefresh = false;
        }
    }

    private void SetOccupy()
    {
        //점령 이펙트 있을시도 처리해주면댐

        LogPanel.Instance.OccupationLog(data.occupyTeam, data.area);
        areaOutlineDic[data.area].SetOccupy(data.occupyTeam);
    }
}
