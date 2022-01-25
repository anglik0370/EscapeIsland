using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    protected Image image;
    protected ItemSO item;

    //아이템 이미지가 몇번째에 있는지
    [SerializeField]
    protected int itemImgDepth;

    public bool IsEmpty => item == null;

    protected ItemGhost itemGhost;
    protected bool isDraging;

    protected virtual void Awake() 
    {
        Image[] imgs = GetComponentsInChildren<Image>();

        image = imgs[itemImgDepth];
        itemGhost = FindObjectOfType<ItemGhost>();

        if(item == null)
        {
            image.sprite = null;
        }
        else
        {
            image.sprite = item.itemSprite;
        }
    }

    public void SetItem(ItemSO item)
    {
        this.item = item;

        if(item == null)
        {
            image.sprite = null;
        }
        else
        {
            image.sprite = item.itemSprite;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(IsEmpty) return;

        //고스트에 이미지 전달
        itemGhost.SetItem(item);
        isDraging = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if(isDraging)
        {
            itemGhost.SetPosition(eventData.position);
        }
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //드롭 오브젝트에서 발생
        
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

    public virtual void OnEndDrag(PointerEventData eventData)
    {   
        //드래그 오브젝트에서 발생

        if(isDraging)
        {
            if(itemGhost.GetItem() != null)
            {
                SetItem(itemGhost.GetItem());
            }
            else
            {
                SetItem(null);
            }
        }

        isDraging = false;
        itemGhost.Init();
    }
}
