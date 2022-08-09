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
            BuffSO so = BuffManager.Instance.GetBuffSO(data.altarBuffId);

            if(so!= null)
            {
                AltarPanel.Instance.SetEffectText(so.buffExplanation);
                so.InitializeBuff(user.gameObject);
            }
        }
        AltarPanel.Instance.ClosePanel(1f);
    }
}
