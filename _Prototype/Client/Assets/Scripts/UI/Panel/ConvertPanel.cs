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

    private bool isBlueLimit = false;
    private bool isRedLimit = false;

    private WaitForSeconds limitWs;

    [SerializeField]
    private Image bgImg;

    [SerializeField]
    private Sprite smelterBG;
    [SerializeField]
    private Sprite refineryBG;

    [SerializeField]
    private Sprite smelterSlot;
    [SerializeField]
    private Sprite refinerySlot;

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        ResetUIs();

        limitWs = new WaitForSeconds(30f);
    }

    protected override void Start()
    {
        base.Start();

        EventManager.SubGameOver(goc =>
        {
            ResetUIs();
        });
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

    public void UpdateUIs()
    {
        if (curOpenConverter == null) return;

        bgImg.sprite = curOpenConverter.IsWater ? refineryBG : smelterBG;

        Sprite slotImg = curOpenConverter.IsWater ? refinerySlot : smelterSlot;

        beforeSlot.SetSlotImage(slotImg);
        afterSlot.SetSlotImage(slotImg);

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
        //앰버 체크
        {
            if (isBlueLimit)
            {
                if (converter.AreaStateHolder.Area.Equals(Area.BlueLobby) && PlayerManager.Instance.Player.CurTeam.Equals(Team.RED))
                {
                    UIManager.Instance.AlertText("현재 사용하실 수 없습니다", AlertType.Warning);
                    return;
                }
            }

            if (isRedLimit)
            {
                if (converter.AreaStateHolder.Area.Equals(Area.RedLobby) && PlayerManager.Instance.Player.CurTeam.Equals(Team.BLUE))
                {
                    UIManager.Instance.AlertText("현재 사용하실 수 없습니다", AlertType.Warning);
                    return;
                }
            }
        }

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

    public void ConvertLimit(Team team)
    {
        StartCoroutine(LimitCo(team.Equals(Team.BLUE)));
    }

    private IEnumerator LimitCo(bool isBlue)
    {
        yield return null;

        if(isBlue)
        {
            isBlueLimit = true;

            yield return limitWs;

            isBlueLimit = false;
        }
        else
        {
            isRedLimit = true;

            yield return limitWs;

            isRedLimit = false;
        }
    }
}
