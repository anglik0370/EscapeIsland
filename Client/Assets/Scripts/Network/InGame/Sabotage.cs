using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sabotage : ISetAble
{
    public SabotageVO sabotageData;

    private bool needSabotageRefresh = false;

    protected override void Start()
    {
        base.Start();

    }

    private void Update()
    {
        if (needSabotageRefresh)
        {
            StartSabotage();
            needSabotageRefresh = false;
        }
    }

    public void SetSabotageData(SabotageVO vo)
    {
        lock(lockObj)
        {
            sabotageData = vo;
            needSabotageRefresh = true;
        }
    }

    public void StartSabotage()
    {
        SabotageButton curSabotage = SabotagePanel.Instance.FindSabotageButton(sabotageData.sabotageName);

        if((sabotageData.isShareCoolTime && user.isKidnapper) || user.isKidnapper)
        {
            curSabotage.StartSabotage();
        }

        curSabotage.SabotageSO.callback?.Invoke();
    }
}
