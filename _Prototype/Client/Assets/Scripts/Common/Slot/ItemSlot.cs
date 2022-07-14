using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public enum SlotKind
    {
        Inventory,
        Storage,
        ConverterBefore,
        ConverterAfter,
        MissionDropItem,
        MissionBatterySlot,
        CantUseRefinery,
        Arson,
        TrashCan,
        MissionStorageDropSlot,
        AltarSlot,
    }

    protected Image image;
    [SerializeField]
    protected ItemSO item;

    //아이템 이미지가 몇번째에 있는지
    [SerializeField]
    protected int itemImgDepth;

    public bool IsEmpty => item == null;

    [SerializeField]
    private SlotKind kind;
    public SlotKind Kind => kind;

    protected virtual void Awake() 
    {
        Image[] imgs = GetComponentsInChildren<Image>();

        image = imgs[itemImgDepth];

        if(item == null)
        {
            image.sprite = null;
            image.color = UtilClass.limpidityColor;
        }
        else
        {
            image.sprite = item.itemSprite;
            image.color = UtilClass.opacityColor;
        }
    }

    public void SetItem(ItemSO item)
    {
        this.item = item;

        if(item == null)
        {
            image.sprite = null;
            image.color = UtilClass.limpidityColor;
        }
        else
        {
            image.sprite = item.itemSprite;
            image.color = UtilClass.opacityColor;
        }
    }

    public ItemSO GetItem()
    {
        return item;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        //드래그 오브젝트에서 발생
        SlotManager.Instance.BeginDrag(this);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //드롭 오브젝트에서 발생
        SlotManager.Instance.EndDrag(this);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //드래그 오브젝트에서 발생
        SlotManager.Instance.EndDrag();
    }
}
