using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerryGhostMObj : MonoBehaviour
{
    private RectTransform rect;
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void Init(Sprite sprite, RectTransform rect)
    {
        img.sprite = sprite;
        rect.anchoredPosition = rect.anchoredPosition;
    }

    public void Move(Vector2 point)
    {
        rect.position = point;
    }
}
