using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoragePanel : Panel
{
    public static StoragePanel Instance;

    [SerializeField]
    private ProgressUI progressUI;
    [SerializeField]
    private List<StorageSlot> slotList = new List<StorageSlot>();

    private bool isGameStart = false;

    protected override void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        slotList = GetComponentsInChildren<StorageSlot>().ToList();
    }

    protected override void Start()
    {
        EventManager.SubGameStart(p =>
        {
            isGameStart = true;
        });

        EventManager.SubGameOver(goc =>
        {
            isGameStart = false;
        });

        base.Start();
    }

    public void UpdateUIs(ItemSO item, float progress)
    {
        ItemAmount maxAmount = StorageManager.Instance.FindItemAmount(true, item);
        ItemAmount curAmount = StorageManager.Instance.FindItemAmount(false, item);

        StorageSlot slot = slotList.Find(x => x.OriginItem.itemId == item.itemId);

        slot.SetAmountText(maxAmount.amount, curAmount.amount);

        progressUI.UpdateProgress(progress);
    }

    public override void Open(bool isTweenSkip = false)
    {
        //Debug.LogWarning("Item을 넣어서 사용하세요");
        Open(null);
    }

    public void Open(ItemSO item)
    {
        if (!isGameStart) return;

        print(item + " 저장소 열기");

        base.Open();

        for(int i = 0; i < slotList.Count; i++)
        {
            ItemAmount maxAmount = StorageManager.Instance.FindItemAmount(true, slotList[i].OriginItem);
            ItemAmount curAmount = StorageManager.Instance.FindItemAmount(false, slotList[i].OriginItem);

            slotList[i].SetAmountText(maxAmount.amount, curAmount.amount);
        }
    }
}
