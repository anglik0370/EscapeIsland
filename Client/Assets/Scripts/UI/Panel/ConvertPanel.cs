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
    private ItemSlot[] cantUseRefinery;
    [SerializeField]
    private GameObject cantUseRefineryParentObj;

    [SerializeField]
    private ItemSO sand;
    public ItemSO SandItem => sand;

    [SerializeField]
    private Image progressArrowImg;
    [SerializeField]
    private Text remainTimeText;

    [SerializeField]
    private Text beforeItemNameText;
    [SerializeField]
    private Text afterItemText;

    [SerializeField]
    private ItemConverter curOpenConverter;
    public ItemConverter CurOpenConverter => curOpenConverter;

    [SerializeField]
    private List<ItemConverter> refineryList;
    public List<ItemConverter> RefineryList => refineryList;

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        ResetUIs();
    }

    protected override void Start()
    {
        base.Start();

        EventManager.SubGameOver(goc =>
        {
            ResetUIs();
            Init();
        });

        EventManager.SubExitRoom(Init);

        refineryList = ConverterManager.Instance.GetRefineryList();
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

    public void Init()
    {
        for (int i = 0; i < refineryList.Count; i++)
        {
            refineryList[i].Init();
        }
    }

    public bool CanUseRefinery()
    {
        return curOpenConverter.CanUseConverter();
    }

    public void StartCantUseRefinery()
    {
        for (int i = 0; i < refineryList.Count; i++)
        {
            refineryList[i].SetCantUse();
        }
        UpdateUIs();
    }

    public int GetRefinerySlotIdx(ItemSlot slot)
    {
        for (int i = 0; i < cantUseRefinery.Length; i++)
        {
            if(cantUseRefinery[i] == slot)
            {
                return i;
            }
        }

        return -1;
    }

    public void ResetOreItem()
    {
        curOpenConverter.ConvertingReset();
    }

    public void UpdateUIs()
    {
        if (curOpenConverter == null) return;

        beforeSlot.SetItem(curOpenConverter.BeforeItem);
        afterSlot.SetItem(curOpenConverter.AfterItem);

        cantUseRefineryParentObj.SetActive(!curOpenConverter.CanUse);

        for (int i = 0; i < cantUseRefinery.Length; i++)
        {
            cantUseRefinery[i].SetItem(curOpenConverter.isEmpty[i] ? null : sand);
        }

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

    public void SetNameText(string before = "(변환 전 재료)", string after = "(변환 후 재료)")
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
