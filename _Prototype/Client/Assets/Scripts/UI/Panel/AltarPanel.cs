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

        offerBtn.onClick.AddListener(OnClickOfferBtn);
    }

    private void OnClickOfferBtn()
    {
        currentProbability = defaultProbability;

        for (int i = 0; i < slots.Count; i++)
        {
            AltarSlot slot = slots[i];

            if(slot.IsEmpty) continue;

            currentProbability += probabilityList[slot.GetItem().tier];
        }

        if (lastBuff != null && !lastBuff.isBuffed)
        {
            currentProbability += lastBuffIsNerfProbability;
        }

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

        SendManager.Instance.SendAltar(new AltarVO(NetworkManager.instance.socketId, buffId));
    }
}
