using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Occupy : ISetAble
{
    public static Occupy Instance { get; private set; }

    [SerializeField]
    private SerializableDictionary<Area, SpriteOutline> mapOutlineDic = new SerializableDictionary<Area, SpriteOutline>();
    [SerializeField]
    private SerializableDictionary<Area, UIOutline> uiOutlineDic = new SerializableDictionary<Area, UIOutline>();

    [SerializeField]
    private List<SpriteOutline> spriteOutlineList = new List<SpriteOutline>();
    [SerializeField]
    private List<UIOutline> uiOutlineList = new List<UIOutline>();

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
            mapOutlineDic.Add((Area)Enum.GetValues(typeof(Area)).GetValue(i), spriteOutlineList[i - 1]);
            uiOutlineDic.Add((Area)Enum.GetValues(typeof(Area)).GetValue(i), uiOutlineList[i - 1]);
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
        mapOutlineDic[data.area].SetOccupy(data.occupyTeam);
        uiOutlineDic[data.area].SetOccupy(data.occupyTeam);
    }
}
