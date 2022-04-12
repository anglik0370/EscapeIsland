using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum BranchMObjType
{
    Horizontal,
    Vertical,
}

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
    private Sprite itemSprite;
    private Sprite originSprite;

    [SerializeField]
    private Vector2 beginPoint;
    [SerializeField]
    private Vector2 endPoint;

    public Vector2 BeginPoint => beginPoint;
    public Vector2 EndPoint => endPoint;

    [SerializeField]
    private BranchMObjType type;
    public BranchMObjType Type => type;

    private void Awake()
    {
        img = GetComponent<Image>();

        rect = GetComponent<RectTransform>();

        beginPoint = transform.Find("BeginPoint").transform.position;
        endPoint = transform.Find("EndPoint").transform.position;
    }

    private void Start()
    {
        originPos = rect.anchoredPosition;
        originSize = rect.sizeDelta;

        originSprite = img.sprite;

        img.raycastTarget = false;
    }

    public void Init()
    {
        rect.anchoredPosition = originPos;
        rect.sizeDelta = originSize;
        img.sprite = originSprite;

        img.raycastTarget = false;
    }

    public void Drop(Vector2 dropPoint)
    {
        img.sprite = itemSprite;

        rect.DOAnchorPos(dropPoint, 0.5f).OnComplete(() => img.raycastTarget = true);
        rect.sizeDelta = new Vector2(100, 100);
    }
}
