using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoragePanel : Panel
{
    public static StoragePanel Instance;

    private ProgressUI progressUI;
    [SerializeField]
    private List<StorageSlot> slotList = new List<StorageSlot>();

    protected override void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        slotList = GetComponentsInChildren<StorageSlot>().ToList();
        progressUI = FindObjectOfType<ProgressUI>();
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
        Debug.LogWarning("Item을 넣어서 사용하세요");
    }

    public void Open(ItemSO item)
    {
        base.Open();

        for(int i = 0; i < slotList.Count; i++)
        {
            ItemAmount maxAmount = StorageManager.Instance.FindItemAmount(true, slotList[i].OriginItem);
            ItemAmount curAmount = StorageManager.Instance.FindItemAmount(false, slotList[i].OriginItem);

            slotList[i].SetAmountText(maxAmount.amount, curAmount.amount);
        }
    }
}
