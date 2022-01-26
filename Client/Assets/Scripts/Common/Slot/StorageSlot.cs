using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StorageSlot : ItemSlot
{
    private Text amountText;

    [SerializeField]
    private ItemSO originItem;

    public ItemSO OriginItem => originItem;

    private StoragePanel storagePanel;

    protected override void Awake()
    {
        base.Awake();

        storagePanel = StoragePanel.Instance;
        amountText = GetComponentInChildren<Text>();

        SetItem(originItem);
    }

    public void SetAmountText(int max, int cur)
    {
        amountText.text = $"{cur} / {max}";
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public override void OnDrag(PointerEventData eventData)
    {
        
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if(itemGhost.GetItem() == null) return;

        if(itemGhost.GetItem().itemId != originItem.itemId)
        {
            //다른 아이템이라면
            return;
        }
        else
        {
            //꽉안찼으면 실행
            if(!storagePanel.IsItemFull(itemGhost.GetItem()))
            {
                NetworkManager.instance.StorageDrop(itemGhost.GetItem().itemId);
                storagePanel.AddItem(itemGhost.GetItem(), this);
                itemGhost.SetItem(null);
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        
    }
}