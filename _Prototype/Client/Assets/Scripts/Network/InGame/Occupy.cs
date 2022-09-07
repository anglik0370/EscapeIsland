using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupy : ISetAble
{
    public static Occupy Instance { get; private set; }

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
    }
}
