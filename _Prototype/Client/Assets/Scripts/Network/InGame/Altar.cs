using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : ISetAble
{
    private AltarVO data;

    private bool isAltar = false;

    public void SetAltarData(AltarVO vo)
    {
        lock(lockObj)
        {
            data = vo;
            isAltar = true;
        }
    }

    private void Update()
    {
        if(isAltar)
        {
            AltarFunc();
            isAltar = false;
        }
    }

    private void AltarFunc()
    {
        if(data.id.Equals(user.socketId))
        {
            AltarPanel.Instance.InitTimer();
            BuffManager.Instance.GetBuffSO(data.altarBuffId)?.InitializeBuff(user.gameObject);
        }
    }
}
