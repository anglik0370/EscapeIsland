using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MSGlassSlot : ItemSlot
{
    public new bool IsEmpty => image.color == UtilClass.limpidityColor;

    protected override void Awake()
    {
        image = GetComponent<Image>();
        image.color = UtilClass.limpidityColor;
    }

    public void Enable()
    {
        image.color = UtilClass.opacityColor;
    }
}
