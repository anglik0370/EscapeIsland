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
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        img.color = UtilClass.limpidityColor;
    }

    public void Init(Sprite sprite, RectTransform rect)
    {
        img.sprite = sprite;
        this.rect.position = rect.position;
        this.rect.sizeDelta = rect.sizeDelta;

        img.color = UtilClass.opacityColor;
    }

    public void Disable()
    {
        img.color = UtilClass.limpidityColor;
    }

    public void Move(Vector2 point)
    {
        rect.position = point;
    }
}
