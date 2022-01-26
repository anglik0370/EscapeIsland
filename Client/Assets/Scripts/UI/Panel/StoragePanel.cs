using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoragePanel : Panel
{
    public static StoragePanel Instance;

    private ProgressUI progressUI;
    private List<StorageSlot> slotList;
    private ItemStorage storage;

    protected override void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        slotList = GetComponentsInChildren<StorageSlot>().ToList();
        storage = FindObjectOfType<ItemStorage>();
        progressUI = FindObjectOfType<ProgressUI>();
    }

    private void Start() 
    {
        for(int i = 0; i < slotList.Count; i++)
        {
            ItemAmount maxAmount = storage.FindItemAmount(storage.maxAmountItemList, slotList[i].OriginItem);
            ItemAmount curAmount = storage.FindItemAmount(storage.curAmountItemList, slotList[i].OriginItem);

            slotList[i].SetAmountText(maxAmount.amount, curAmount.amount);
        }
    }

    public void AddItem(ItemSO item, StorageSlot slot)
    {
        storage.AddItem(item);

        ItemAmount maxAmount = storage.FindItemAmount(storage.maxAmountItemList, item);
        ItemAmount curAmount = storage.FindItemAmount(storage.curAmountItemList, item);

        slot.SetAmountText(maxAmount.amount, curAmount.amount);

        progressUI.UpdateProgress(storage.GetProgress());
    }

    public bool IsItemFull(ItemSO item)
    {
        return storage.IsItemFull(item);
    }

    public override void Open()
    {
        base.Open();

        for(int i = 0; i < slotList.Count; i++)
        {
            ItemAmount maxAmount = storage.FindItemAmount(storage.maxAmountItemList, slotList[i].OriginItem);
            ItemAmount curAmount = storage.FindItemAmount(storage.curAmountItemList, slotList[i].OriginItem);

            slotList[i].SetAmountText(maxAmount.amount, curAmount.amount);
        }
    }
}
