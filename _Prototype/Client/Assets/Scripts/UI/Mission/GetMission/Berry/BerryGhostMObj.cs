using DG.Tweening;
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

    public void Disable(bool isTweening)
    {
        if(isTweening)
        {
            img.DOColor(UtilClass.limpidityColor, 0.5f);
        }
        else
        {
            img.color = UtilClass.limpidityColor;
        }
    }

    public void Move(Vector3 pos)
    {
        pos.z = 10.0f;
        
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}
