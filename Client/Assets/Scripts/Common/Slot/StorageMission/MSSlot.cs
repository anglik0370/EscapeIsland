using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MSSlot : ItemSlot
{
    private IStorageMission mission;
    public IStorageMission Mission => mission;

    public new bool IsEmpty => image.color == UtilClass.limpidityColor;

    protected override void Awake()
    {
        mission = GetComponentInParent<IStorageMission>();

        image = GetComponent<Image>();
        image.color = UtilClass.limpidityColor;
    }

    public void EnableImg()
    {
        image.color = UtilClass.opacityColor;
    }

    public void DisableImg()
    {
        image.color = UtilClass.limpidityColor;
    }
}
