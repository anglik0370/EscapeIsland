using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField]
    private bool isEnable = true;
    public bool IsEnable => isEnable;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        missionBerry = GetComponentInParent<MissionBerry>();

        originPos = rect.anchoredPosition;
        isEnable = true;
    }

    private void Start()
    {
        originPos = rect.anchoredPosition;
        isEnable = true;
    }

    public void Init()
    {
        rect.anchoredPosition = originPos;
        img.color = UtilClass.opacityColor;
        img.raycastTarget = true;
        isEnable = true;
    }

    public void Disable()
    {
        isEnable = false;

        img.DOColor(UtilClass.limpidityColor, 0.5f);
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
        if(isEnable)
        {
            img.color = UtilClass.opacityColor;
        }

        missionBerry.EndDrag(false);
    }
}
