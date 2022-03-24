using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AfterSlot : ItemSlot
{
    private ConvertPanel convertPanel;

    private Inventory inventory;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start() 
    {
        convertPanel = ConvertPanel.Instance;

        EventManager.SubEnterRoom(p =>
        {
            inventory = p.inventory;
        });
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
        convertPanel.TakeIngotItem();
        //제련된 아이템을 가져간 상태

        SendManager.Instance.TakeConverterAfterItem(convertPanel.CurOpenConverter.id);
    }
}
