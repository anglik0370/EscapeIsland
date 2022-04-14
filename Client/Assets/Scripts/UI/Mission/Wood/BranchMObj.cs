using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BranchMObj : MonoBehaviour
{
    private Image img;

    private RectTransform rect;

    [SerializeField]
    private Vector2 originPos;
    [SerializeField]
    private Vector2 originSize;

    [SerializeField]
    private int id;
    public int Id => id;

    [SerializeField]
    private bool isDropped = false;
    public bool IsDropped => isDropped;

    [SerializeField]
    private Sprite itemSprite;
    private Sprite originSprite;

    [SerializeField]
    private Vector2 beginPoint;
    [SerializeField]
    private Vector2 endPoint;

    public Vector2 BeginPoint => beginPoint;
    public Vector2 EndPoint => endPoint;

    private void Awake()
    {
        img = GetComponent<Image>();

        rect = GetComponent<RectTransform>();

        beginPoint = transform.Find("BeginPoint").GetComponent<RectTransform>().position;
        endPoint = transform.Find("EndPoint").GetComponent<RectTransform>().position;

        originSprite = img.sprite;

        originPos = rect.anchoredPosition;
        originSize = rect.sizeDelta;
    }

    private void Start()
    {
        img.raycastTarget = false;
    }

    public void Init()
    {
        rect.anchoredPosition = originPos;
        rect.sizeDelta = originSize;
        img.sprite = originSprite;

        img.raycastTarget = false;

        isDropped = false;
    }

    public void Drop(Vector2 dropPoint)
    {
        img.sprite = itemSprite;

        img.color = UtilClass.limpidityColor;
        img.DOColor(UtilClass.opacityColor, 0.5f);

        rect.DOAnchorPos(dropPoint, 0.8f).OnComplete(() => img.raycastTarget = true).SetEase(Ease.InCubic);
        rect.sizeDelta = new Vector2(100, 100);

        isDropped = true;
    }
}
