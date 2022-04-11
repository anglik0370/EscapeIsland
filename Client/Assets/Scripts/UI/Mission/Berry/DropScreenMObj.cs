using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropScreenMObj : MonoBehaviour, IDragHandler, IDropHandler
{
    private MissionBerry missionBerry;

    private void Awake()
    {
        missionBerry = GetComponentInParent<MissionBerry>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        missionBerry.EndDrag(false);
    }
}
