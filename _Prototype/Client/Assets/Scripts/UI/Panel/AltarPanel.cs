using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AltarPanel : Panel
{
    public static AltarPanel Instance { get; private set; }

    [SerializeField]
    private Transform slotParentTrm;
    private List<AltarSlot> slots = new List<AltarSlot>();

    [SerializeField]
    private Button offerBtn;

    [SerializeField]
    private Image probabilityFillImg;

    [SerializeField]
    private Text probabilityText;
    [SerializeField]
    private Text effectText;

    [SerializeField]
    private int defaultProbability = 30;
    private int currentProbability = 30;

    private float panelOffTime = 0.2f;

    private WaitForSeconds panelOff;

    [SerializeField]
    private int lastBuffIsNerfProbability = 5;
    private List<int> probabilityList;

    private BuffSO lastBuff = null;

    [Header("AltarTimter")]
    [SerializeField]
    private float maxTime = 30f;
    private float remainTime = 30f;

    private bool isAltarAble = false;
    public bool IsAltarAble => isAltarAble;

    public bool IsAltarPanelOpen { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        slots = slotParentTrm.GetComponentsInChildren<AltarSlot>().ToList();

        probabilityList = new List<int>()
        {
            0,15,20,25
        };

        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        panelOff = new WaitForSeconds(panelOffTime);

        InitTimer(true);
        SetProbability();
        offerBtn.onClick.AddListener(() => OnClickOfferBtn());
    }

    private void Update()
    {
        if(!isAltarAble)
        {
            remainTime -= Time.deltaTime;

            if(remainTime <= 0f)
            {
                isAltarAble = true;
            }
        }
    }

    #region AltarTimer
    public float GetAmount()
    {
        return remainTime / maxTime;
    }

    public void InitTimer(bool isStart = false)
    {
        if(isStart)
        {
            remainTime = 0f;
            isAltarAble = true;
            return;
        }

        remainTime = maxTime;
        isAltarAble = false;
    }
    #endregion

    public void ClosePanel(float time)
    {
        InitTimer();

        if (!IsAltarPanelOpen) return;

        panelOffTime = time;
        panelOff = new WaitForSeconds(time);

        StartCoroutine(PanelOff());
    }

    public void SetProbability()
    {
        currentProbability = defaultProbability;

        for (int i = 0; i < slots.Count; i++)
        {
            AltarSlot slot = slots[i];

            if (slot.IsEmpty) continue;

            currentProbability += probabilityList[slot.GetItem().tier];
        }

        if (lastBuff != null && !lastBuff.isBuffed)
        {
            currentProbability += lastBuffIsNerfProbability;
        }

        if (currentProbability > 100) currentProbability = 100;

        SetFillImg();
        probabilityText.text = $"{currentProbability}%";
    }

    public void SetEffectText(string msg)
    {
        effectText.text = msg;
    }

    private void SetFillImg()
    {
        probabilityFillImg.fillAmount = (float)currentProbability / 100;
    }

    private void OnClickOfferBtn()
    {
        if(currentProbability <= defaultProbability)
        {
            UIManager.Instance.AlertText("최소 한개 이상의 재료가 필요합니다.", AlertType.Warning);
            return;
        }

        int idx = Random.Range(1, 101);
        int buffId = -1;

        if (idx <= currentProbability)
        {
            //버프 돌리기
            buffId = BuffManager.Instance.GetAltarBuffId();
        }
        else
        {
            effectText.text = "꽝";
        }

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetItem(null);
        }

        SendManager.Instance.SendAltar(new AltarVO(NetworkManager.instance.socketId, buffId));
    }

    IEnumerator PanelOff()
    {
        yield return panelOff;

        Close();
    }

    #region override 

    public override void Open(bool isTweenSkip = false)
    {
        base.Open(isTweenSkip);

        IsAltarPanelOpen = true;
    }

    public override void Close(bool isTweenSkip = false)
    {
        base.Close(isTweenSkip);
        IsAltarPanelOpen = false;
    }
    #endregion
}
