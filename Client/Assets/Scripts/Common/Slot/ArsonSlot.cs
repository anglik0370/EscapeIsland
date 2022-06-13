using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArsonSlot : ItemSlot
{
    public int id;
    public bool isArson = false;

    public Image backgroundImg;

    private void OnEnable()
    {
        SetItem(null);
    }

    public void SetActive(Color c,bool enabled = false)
    {
        backgroundImg.color = c;
        image.raycastTarget = enabled;
        backgroundImg.raycastTarget = enabled;
    }

    public void SetArson(bool on)
    {
        isArson = on;
        SetActive(on ? UtilClass.opacityColor : UtilClass.limpidityColor,on);
    }
}
