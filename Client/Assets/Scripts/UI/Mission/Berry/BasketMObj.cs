using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasketMObj : MonoBehaviour, IDragHandler, IDropHandler
{
    private Image img;

    private MissionBerry missionBerry;

    private void Awake()
    {
        img = GetComponent<Image>();
        missionBerry = GetComponentInParent<MissionBerry>();
    }

    public void Init()
    {
        img.color = UtilClass.opacityColor;
    }

    public void Disable()
    {
        img.DOColor(UtilClass.limpidityColor, 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        missionBerry.EndDrag(true);
    }
}
