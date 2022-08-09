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

    [SerializeField]
    private int lastBuffIsNerfProbability = 5;
    private List<int> probabilityList;

    private BuffSO lastBuff = null;

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
        SetProbability();
        offerBtn.onClick.AddListener(OnClickOfferBtn);
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

        SetFillImg();
        probabilityText.text = $"{currentProbability}%";
    }

    private void SetFillImg()
    {
        probabilityFillImg.fillAmount = (float)currentProbability / 100;
    }

    private void OnClickOfferBtn()
    {
        if(currentProbability <= defaultProbability)
        {
            UIManager.Instance.AlertText("�ּ� �Ѱ� �̻��� ��ᰡ �ʿ��մϴ�.", AlertType.Warning);
            return;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetItem(null);
        }

        int idx = Random.Range(1, 101);
        int buffId = -1;

        if (idx <= currentProbability)
        {
            //���� ������
            buffId = BuffManager.Instance.GetAltarBuffId();
        }
        else
        {
            effectText.text = "��";
        }

        SendManager.Instance.SendAltar(new AltarVO(NetworkManager.instance.socketId, buffId));
    }
}
