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

    public RectTransform Rect => rect;
    public Sprite ItemSprite => img.sprite;

    [SerializeField]
    private Vector2 originPos;

    [SerializeField]
    private int berryId;
    public int BerryId => berryId;

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

    public void MoveBasketPoint(Vector3 point)
    {
        rect.position = point;
        img.raycastTarget = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        missionBerry.SetCurBerryObj(this);
        img.color = UtilClass.limpidityColor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        missionBerry.MoveBerryGhost(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        img.color = UtilClass.opacityColor;
    }
}
