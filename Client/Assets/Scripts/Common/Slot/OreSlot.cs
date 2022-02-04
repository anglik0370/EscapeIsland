using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OreSlot : ItemSlot
{
    private RefineryPanel refineryPanel;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start() 
    {
        refineryPanel = RefineryPanel.Instance;
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

        if(itemGhost.GetItem().canRefining)
        {
            NetworkManager.instance.StartRefinery(refineryPanel.NowOpenRefinery.id, itemGhost.GetItem().itemId);
            //refineryPanel.SetOreItem(itemGhost.GetItem());
            base.OnDrop(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(itemGhost.GetItem().canRefining)
        {
            NetworkManager.instance.ResetRefinery(refineryPanel.NowOpenRefinery.id);

            refineryPanel.ResetOreItem(itemGhost.GetItem());
            base.OnEndDrag(eventData);
        }
    }
}
