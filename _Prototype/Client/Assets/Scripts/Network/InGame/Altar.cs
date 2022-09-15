using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : ISetAble
{
    private AltarVO data;

    private bool isAltar = false;

    [SerializeField]
    private AudioClip successClip;
    [SerializeField]
    private AudioClip failClip;

    public void SetAltarData(AltarVO vo)
    {
        lock (lockObj)
        {
            data = vo;
            isAltar = true;
        }
    }

    private void Update()
    {
        if (isAltar)
        {
            AltarFunc();
            isAltar = false;
        }
    }

    private void AltarFunc()
    {
        if (data.id.Equals(user.socketId))
        {
            BuffSO so = BuffManager.Instance.GetBuffSO(data.altarBuffId);

            if (so != null)
            {
                AltarPanel.Instance.SetEffectText(so.buffExplanation);
                user.BuffHandler.AddBuff(so.InitializeBuff(user.gameObject));

                SoundManager.Instance.PlaySFX(successClip);
            }
            else
            {
                SoundManager.Instance.PlaySFX(failClip);
            }
        }
        AltarPanel.Instance.ClosePanel(1f);
    }
}
