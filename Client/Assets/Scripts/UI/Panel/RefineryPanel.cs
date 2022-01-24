using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineryPanel : Panel
{
    public static RefineryPanel Instance;

    public Sprite noneItemSprite;

    public Image oreSlotImg;
    public Text oreNameText;

    public Image ingotSlotImg;
    public Text ingotNameText;

    public Image progressArrowImg;
    public Text progressTimeText;

    public Refinery nowOpenRefinery;

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();
    }

    private void Update() 
    {
        if(nowOpenRefinery != null)
        {
            if(nowOpenRefinery.isRefiningEnd)
            {
                //광석 스프라이트 바꿔주고
                oreSlotImg.sprite = nowOpenRefinery.oreItem.itemSprite;
                ingotSlotImg.sprite = noneItemSprite;

                //시간도 바꿔주고
                progressTimeText.text = Mathf.RoundToInt(nowOpenRefinery.remainTime).ToString();

                //화살표도 채워주고
                progressArrowImg.fillAmount = nowOpenRefinery.remainTime / nowOpenRefinery.refiningTime;
            }
            else
            {
                //재련이 끝나면
                oreSlotImg.sprite = noneItemSprite;
                ingotSlotImg.sprite = nowOpenRefinery.ingotItem.itemSprite;
            }
        }
    }

    public void Open(Refinery refinery)
    {
        base.Open();

        nowOpenRefinery = refinery;
    }

    public override void Close()
    {
        base.Close();

        nowOpenRefinery = null;
    }
}
