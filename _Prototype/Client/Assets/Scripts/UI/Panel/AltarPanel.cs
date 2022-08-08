using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AltarPanel : Panel
{
    [SerializeField]
    private int maxItemCnt = 0;
    [SerializeField]
    private int itemCnt = 0;

    [SerializeField]
    private Transform slotParentTrm;
    private List<AltarSlot> slots = new List<AltarSlot>();

    [SerializeField]
    private Button offerBtn;

    [SerializeField]
    private int defaultProbability = 30;
    private int currentProbability = 30;

    protected override void Awake()
    {
        base.Awake();

        slots = slotParentTrm.GetComponentsInChildren<AltarSlot>().ToList();

        maxItemCnt = slots.Count;
    }

    protected override void Start()
    {
        base.Start();

        offerBtn.onClick.AddListener(OnClickOfferBtn);
    }

    private void OnClickOfferBtn()
    {
        if(AltarSlotsIsNull())
        {
            UIManager.Instance.AlertText("최소 한개 이상의 재료가 필요합니다.", AlertType.Warning);
            return;
        }

        int idx = Random.Range(1, 101);

        if(idx <= currentProbability)
        {
            //버프 돌리기
        }
        else
        {

        }
    }

    private bool AltarSlotsIsNull()
    {
        int count = 0;

        for (int i = 0; i < slots.Count; i++)
        {
            if(!slots[i].IsEmpty)
            {
                count++;
            }
        }

        return count <= 0;
    }

    public void DropItem()
    {
        if(itemCnt < maxItemCnt)
        {
            itemCnt++;
        }
        else
        {
            itemCnt = 0;

            //슬롯 초기화 해주고
            slots.ForEach(x => x.SetItem(null));

            //여기서 버프를 주면 됨

        }
    }
}
