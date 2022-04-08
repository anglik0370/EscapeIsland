using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasketMObj : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private MissionBerry missionBerry;

    private void Awake()
    {
        missionBerry = GetComponentInParent<MissionBerry>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        missionBerry.MoveToBasketTrm();
    }
}
