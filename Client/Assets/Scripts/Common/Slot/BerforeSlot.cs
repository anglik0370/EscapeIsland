using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BerforeSlot : ItemSlot
{
    private ConvertPanel convertPanel;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start() 
    {
        convertPanel = ConvertPanel.Instance;
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

        if (convertPanel.CurOpenConverter.IsConverting) return;
        if (convertPanel.CurOpenConverter.AfterItem != null) return;

        if(itemGhost.GetItem().canRefining)
        {
            SendManager.Instance.StartConverting(convertPanel.CurOpenConverter.id, itemGhost.GetItem().itemId);
            base.OnDrop(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        SendManager.Instance.ResetConverter(convertPanel.CurOpenConverter.id);
        base.OnEndDrag(eventData);
    }
}
