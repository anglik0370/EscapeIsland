using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitlePanel : MonoBehaviour
{
    private Button btn;
    private Image img;

    private RectTransform imageRect;
    private RectTransform textRect;

    [SerializeField]
    private float imageMinWidth;
    [SerializeField]
    private float imageMinHeight;

    private float originMinWidth;
    private float originMinHeight;

    [SerializeField]
    private float textDownYPos;

    private float originYPos;

    private Sequence seq;

    private void Awake()
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();

        imageRect = transform.Find("TitleImage").GetComponent<RectTransform>();
        textRect = transform.Find("TitleText").GetComponent<RectTransform>();

        originMinWidth = imageRect.offsetMax.x;
        originMinHeight = imageRect.offsetMax.y;

        originYPos = textRect.anchoredPosition.y;

        btn.onClick.AddListener(Close);
    }

    public void Close()
    {
        if(seq != null)
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        seq.Append(DOTween.To(() => imageRect.offsetMax, x => imageRect.offsetMax = x, new Vector2(-imageMinWidth, -imageMinHeight), 1f));
        seq.Join(textRect.DOAnchorPosY(textDownYPos, 1f));

        img.raycastTarget = false;
    }
}
