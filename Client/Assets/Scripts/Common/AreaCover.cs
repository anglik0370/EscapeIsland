using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AreaCover : MonoBehaviour
{
    private SpriteRenderer sr;

    private Color coverColor;
    private Color disCoverColor;

    private const float DURATION = 1f;
    private bool isCovered = true; //덮여있는지

    private Sequence seq;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();

        seq = DOTween.Sequence();

        coverColor = sr.color;
        disCoverColor = new Color(coverColor.r, coverColor.g, coverColor.b, 0);
    }

    public void Enter()
    {
        if(isCovered)
        {
            seq.Kill();
            seq = DOTween.Sequence();
            seq.Append(sr.DOColor(disCoverColor, DURATION));
        }
        else
        {
            seq.Kill();
            seq = DOTween.Sequence();
            seq.Append(sr.DOColor(coverColor, DURATION));
        }

        isCovered = !isCovered;
    }
}
