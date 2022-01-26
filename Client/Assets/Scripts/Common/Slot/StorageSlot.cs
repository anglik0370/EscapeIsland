using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageSlot : ItemSlot
{
    public ItemSO originItem;

    protected override void Awake()
    {
        base.Awake();
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
        if(itemGhost.GetItem().itemId != originItem.itemId)
        {
            //다른 아이템이라면
            return;
        }
        else
        {
            if(itemGhost.GetItem() != null)
            {
                ItemSO temp = item;
                SetItem(itemGhost.GetItem());
                itemGhost.SetItem(temp);
            }
            else
            {
                itemGhost.SetItem(null);
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }
}
