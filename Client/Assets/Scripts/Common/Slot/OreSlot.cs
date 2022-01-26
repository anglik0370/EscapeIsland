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

        if(!itemGhost.GetItem().canRefining)
        {
            //재련할 수 없는 아이템이라면
            return;
        }
        else
        {
            refineryPanel.SetOreItem(itemGhost.GetItem());
            base.OnDrop(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }
}
