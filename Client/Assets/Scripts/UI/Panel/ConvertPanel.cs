using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvertPanel : Panel
{
    public static ConvertPanel Instance;

    [SerializeField]
    private ItemSlot beforeSlot;
    [SerializeField]
    private ItemSlot afterSlot;

    [SerializeField]
    private Image progressArrowImg;
    [SerializeField]
    private Text remainTimeText;

    [SerializeField]
    private Text beforeItemNameText;
    [SerializeField]
    private Text afterItemText;

    private ItemConverter curOpenConverter;
    public ItemConverter CurOpenConverter => curOpenConverter;

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        ResetUIs();
    }

    private void Start()
    {
        EventManager.SubGameOver(goc =>
        {
            ResetUIs();

            Close(true);
        });

        EventManager.SubStartMeet(mt => Close(true));
    }

    private void Update() 
    {
        if(curOpenConverter == null) return;
        if (!curOpenConverter.IsConverting)
        {
            ResetUIs();
            return; //변환중이 아니면 갱신할 필요가 없다
        }

        //텍스트 업데이트

        //이미지 업데이트
        SetArrowProgress(1 - (curOpenConverter.RemainTime / curOpenConverter.ConvertingTime));
        SetTimerText($"{Mathf.RoundToInt(curOpenConverter.RemainTime)}초");
    }

    public void ResetOreItem()
    {
        curOpenConverter.ConvertingReset();
    }

    public void TakeIngotItem()
    {
        curOpenConverter.SetAfterItem(null);
    }

    public void UpdateUIs()
    {
        beforeSlot.SetItem(curOpenConverter.BeforeItem);
        afterSlot.SetItem(curOpenConverter.AfterItem);

        if (curOpenConverter.IsConverting)
        {
            SetNameText(curOpenConverter.BeforeItem.ToString(), curOpenConverter.FindAfterItem(curOpenConverter.BeforeItem).ToString());
            SetTimerText($"{Mathf.RoundToInt(curOpenConverter.RemainTime)}초");
        }
    }

    public void ResetUIs()
    {
        SetNameText();
        SetTimerText();
        SetArrowProgress();
    }

    public void SetNameText(string before = "(재련할 재료)", string after = "(재련된 재료)")
    {
        beforeItemNameText.text = before;
        afterItemText.text = after;
    }

    public void SetTimerText(string str = "")
    {
        remainTimeText.text = str;
    }

    public void SetArrowProgress(float progress = 0f)
    {
        progressArrowImg.fillAmount = progress;
    }

    public void Open(ItemConverter converter)
    {
        base.Open();

        curOpenConverter = converter;

        UpdateUIs();
    }

    public override void Close(bool isTweenSkip = false)
    {
        base.Close();

        curOpenConverter = null;
    }

    public bool IsOpenRefinery(ItemConverter converter)
    {
        return curOpenConverter == converter;
    }
}
