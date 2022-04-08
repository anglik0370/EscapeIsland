using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BerryMObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rect;
    private Image img;

    private MissionBerry missionBerry;

    [SerializeField]
    private Vector2 originPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        missionBerry = GetComponentInParent<MissionBerry>();
    }

    private void Start()
    {
        originPos = rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        missionBerry.SetCurMovingObj(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //rect.anchoredPosition = originPos;
    }
}
