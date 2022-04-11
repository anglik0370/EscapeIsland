using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionDropItemSlot : ItemSlot
{
    private Color visibleColor = Color.white;
    private Color invisibleColor = new Color(0, 0, 0, 0);

    protected override void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Init()
    {
        image.color = visibleColor;
    }

    public void SetRaycastTarget(bool value)
    {
        image.raycastTarget = value;
    }

    public void Disable()
    {
        image.color = invisibleColor;
        image.raycastTarget = false;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        image.color = invisibleColor;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        image.color = image.raycastTarget ? visibleColor : invisibleColor;
    }
}
