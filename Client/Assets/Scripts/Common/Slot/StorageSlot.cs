using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageSlot : ItemSlot
{
    [SerializeField]
    private ItemSO originItem;

    private StoragePanel storagePanel;

    protected override void Awake()
    {
        base.Awake();

        storagePanel = StoragePanel.Instance;

        SetItem(originItem);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
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
                storagePanel.AddItem(itemGhost.GetItem());
                itemGhost.SetItem(null);
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }
}
