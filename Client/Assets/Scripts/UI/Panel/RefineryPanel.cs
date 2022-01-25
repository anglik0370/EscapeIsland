using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineryPanel : Panel
{
    public static RefineryPanel Instance;

    public ItemSlot oreSlot;
    public ItemSlot ingotSlot;

    [SerializeField]
    private Image progressArrowImg;
    [SerializeField]
    private Text remainTimeText;

    [SerializeField]
    private Text oreNameText;
    [SerializeField]
    private Text ingotNameText;

    private Refinery nowOpenRefinery;

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        SetNameText("(재련할 재료)", "(재련된 재료)");
        SetTimerText("");
        SetArrowProgress(0f);
    }

    private void Update() 
    {
        if(nowOpenRefinery == null) return;
        if(nowOpenRefinery.isRefiningEnd) return;

        //텍스트 업데이트

        //이미지 업데이트
        SetArrowProgress(1 - (nowOpenRefinery.remainTime / nowOpenRefinery.refiningTime));
        SetTimerText($"{Mathf.RoundToInt(nowOpenRefinery.remainTime).ToString()}초");
    }

    public void SetOreItem(ItemSO item)
    {   
        //재련가능한 아이템인지는 슬롯에서 체크해준다
        nowOpenRefinery.StartRefining(item);
    }

    public void TakeIngotItem()
    {
        nowOpenRefinery.ingotItem = null;
    }

    public void UpdateImg()
    {
        oreSlot.SetItem(nowOpenRefinery.oreItem);
        ingotSlot.SetItem(nowOpenRefinery.ingotItem);
    }

    public void SetNameText(string ore, string ingot)
    {
        oreNameText.text = ore;
        ingotNameText.text = ingot;
    }

    public void SetTimerText(string str)
    {
        remainTimeText.text = str;
    }

    public void SetArrowProgress(float progress)
    {
        progressArrowImg.fillAmount = progress;
    }

    public void Open(Refinery refinery)
    {
        base.Open();

        nowOpenRefinery = refinery;

        UpdateImg();

        if(!refinery.isRefiningEnd)
        {
            SetNameText(refinery.oreItem.ToString(), refinery.FindIngotFromOre(refinery.oreItem).ToString());
            SetTimerText($"{Mathf.RoundToInt(nowOpenRefinery.remainTime).ToString()}초");
        }
    }

    public override void Close()
    {
        base.Close();

        nowOpenRefinery = null;

        SetNameText("(재련할 재료)", "(재련된 재료)");
        SetTimerText("");
        SetArrowProgress(0f);
    }

    public bool IsOpenRefinery(Refinery refinery)
    {
        return nowOpenRefinery == refinery;
    }
}
