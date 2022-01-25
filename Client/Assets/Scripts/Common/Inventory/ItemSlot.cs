using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private Image image;
    public ItemSO item;

    public bool IsEmpty => item == null;

    private ItemGhost itemGhost;
    private bool isDraging;

    private void Awake() 
    {
        Image[] imgs = GetComponentsInChildren<Image>();

        image = imgs[1];
        itemGhost = FindObjectOfType<ItemGhost>();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(IsEmpty) return;

        //고스트에 이미지 전달
        itemGhost.SetItem(item);
        isDraging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDraging)
        {
            itemGhost.SetPosition(eventData.position);
        }
    }

    public void OnDrop(PointerEventData eventData)
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

    public void OnEndDrag(PointerEventData eventData)
    {
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
