using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoragePanel : Panel
{
    public static StoragePanel Instance;

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
    }

    public void AddItem(ItemSO item)
    {
        storage.AddItem(item);
    }

    public bool IsItemFull(ItemSO item)
    {
        return storage.IsItemFull(item);
    }
}
