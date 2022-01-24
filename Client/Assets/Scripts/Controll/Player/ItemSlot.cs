using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Image image;
    public ItemSO item;

    public bool IsEmpty => item == null;

    private void Awake() 
    {
        image = GetComponent<Image>();
    }

    public void SetItem(ItemSO item)
    {
        this.item = item;
        image.sprite = item.itemSprite;
    }
}
