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
            ItemAmount maxAmount = storage.FindItemAmount(storage.MaxAmountItemList, slotList[i].OriginItem);
            ItemAmount curAmount = storage.FindItemAmount(storage.CurAmountItemList, slotList[i].OriginItem);

            slotList[i].SetAmountText(maxAmount.amount, curAmount.amount);
        }

        EventManager.SubGameOver(gos => Close(true));

        EventManager.SubStartMeet(mt => Close(true));
    }

    public void AddItem(ItemSO item)
    {
        storage.AddItem(item);

        if (IsItemFull(item))
        {
            Debug.Log($"{item}����");
        }

        if (IsItemFull())
        {
            //��á���� ��á�ٰ� ������ ������� �Ѵ�.
            DataVO dataVO = new DataVO("STORAGE_FULL", "");

            Debug.Log("����");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }

        UpdateUIs(item);
    }

    public void UpdateUIs(ItemSO item)
    {
        ItemAmount maxAmount = storage.FindItemAmount(storage.MaxAmountItemList, item);
        ItemAmount curAmount = storage.FindItemAmount(storage.CurAmountItemList, item);

        StorageSlot slot = slotList.Find(x => x.OriginItem.itemId == item.itemId);

        slot.SetAmountText(maxAmount.amount, curAmount.amount);

        progressUI.UpdateProgress(storage.GetProgress());
    }

    public bool IsItemFull(ItemSO item)
    {
        return storage.IsItemFull(item);
    }

    public bool IsItemFull()
    {
        return storage.IsItemFull();
    }

    public override void Open(bool isTweenSkip = false)
    {
        base.Open();

        for(int i = 0; i < slotList.Count; i++)
        {
            ItemAmount maxAmount = storage.FindItemAmount(storage.MaxAmountItemList, slotList[i].OriginItem);
            ItemAmount curAmount = storage.FindItemAmount(storage.CurAmountItemList, slotList[i].OriginItem);

            slotList[i].SetAmountText(maxAmount.amount, curAmount.amount);
        }
    }
}
