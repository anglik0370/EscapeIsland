using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CoconutPalm : MonoBehaviour
{
    private RectTransform rect;
    private Image img;
    private MissionDropItemSlot slot;

    [SerializeField]
    private bool isDroped;
    public bool IsDropped => isDroped;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        slot = GetComponent<MissionDropItemSlot>();
    }

    public void Init()
    {
        isDroped = false;
        img.raycastTarget = false;

        slot.Init();
    }

    public void Drop(float y)
    {
        isDroped = true;
        img.raycastTarget = true;

        rect.DOAnchorPosY(y, 0.5f);
    }
}
