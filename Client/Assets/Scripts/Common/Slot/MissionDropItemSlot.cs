using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionDropItemSlot : ItemSlot
{
    private Sprite originSprite;
    private Sprite itemSprite;

    protected override void Awake()
    {
        image = GetComponent<Image>();

        originSprite = image.sprite;
        itemSprite = item.itemSprite;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        image.sprite = null;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        image.sprite = image.raycastTarget ? originSprite : null;
    }

    public void Disable()
    {
        image.raycastTarget = false;
        image.sprite = null;
    }
}
