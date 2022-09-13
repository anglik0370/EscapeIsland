using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitlePanel : MonoBehaviour
{
    [SerializeField]
    private Button btn;

    [SerializeField]
    private RectTransform topPanelRect;
    [SerializeField]
    private RectTransform bottomPanelRect;

    [SerializeField]
    private CanvasGroup titleCvs;
    [SerializeField]
    private CanvasGroup loginCvs;

    [SerializeField]
    private CanvasScaler cvsScaler;

    private Sequence seq;

    private void Awake()
    {
        cvsScaler = GetComponentInParent<CanvasScaler>();

        btn.onClick.AddListener(Close);

        Init();
    }

    public void Init()
    {
        btn.interactable = true;

        if (seq != null)
        {
            seq.Kill();
        }

        topPanelRect.SetTop(0);
        topPanelRect.SetBottom(0);
        topPanelRect.SetLeft(0);
        topPanelRect.SetRight(0);

        bottomPanelRect.SetTop(cvsScaler.referenceResolution.y);
        bottomPanelRect.SetBottom(-cvsScaler.referenceResolution.y);
        bottomPanelRect.SetLeft(0);
        bottomPanelRect.SetRight(0);

        UtilClass.SetCanvasGroup(titleCvs);
        UtilClass.SetCanvasGroup(loginCvs);
    }

    public void Close()
    {
        btn.interactable = false;

        if(seq != null)
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        seq.Append(DOTween.To(() => topPanelRect.offsetMax, x => topPanelRect.offsetMax = x, new Vector2(topPanelRect.offsetMax.x, cvsScaler.referenceResolution.y), 1f).SetEase(Ease.InCubic));
        seq.Join(DOTween.To(() => topPanelRect.offsetMin, x => topPanelRect.offsetMin = x, new Vector2(topPanelRect.offsetMin.x, cvsScaler.referenceResolution.y), 1f).SetEase(Ease.InCubic));
        seq.Join(DOTween.To(() => bottomPanelRect.offsetMax, x => bottomPanelRect.offsetMax = x, new Vector2(bottomPanelRect.offsetMax.x, 0), 1f).SetEase(Ease.InCubic));
        seq.Join(DOTween.To(() => bottomPanelRect.offsetMin, x => bottomPanelRect.offsetMin = x, new Vector2(bottomPanelRect.offsetMin.x, -0), 1f).SetEase(Ease.InCubic));
        seq.Append(titleCvs.DOFade(1f, 1f));
        seq.Join(loginCvs.DOFade(1f, 1f));
        seq.AppendCallback(() => {
            titleCvs.interactable = true;
            titleCvs.blocksRaycasts = true;
            loginCvs.interactable = true;
            loginCvs.blocksRaycasts = true;
        });
    }
}
