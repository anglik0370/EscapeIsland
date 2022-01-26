using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngotSlot : ItemSlot
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
        //아이템을 가져갈수만 있는 슬롯이니 리턴
        return;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        refineryPanel.TakeIngotItem();
        //제련된 아이템을 가져간 상태
        NetworkManager.instance.EndRefinery(refineryPanel.NowOpenRefinery.id);
        RefineryPanel.Instance.SetNameText("(재련할 재료)", "(재련된 재료)");
        RefineryPanel.Instance.SetTimerText("");
    }
}
