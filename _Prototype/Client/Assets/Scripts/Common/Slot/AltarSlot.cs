using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarSlot : ItemSlot
{
    public new void SetItem(ItemSO item)
    {
        this.item = item;

        if (item == null)
        {
            image.sprite = null;
            image.color = UtilClass.limpidityColor;
        }
        else
        {
            image.sprite = item.itemSprite;
            image.color = UtilClass.opacityColor;
        }

        AltarPanel.Instance.SetProbability();
    }
}
