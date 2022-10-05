using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Panel : MonoBehaviour
{
    protected const float TWEEN_DURATION = 0.5f;
    protected CanvasGroup cvs;
    protected Sequence seq;

    protected virtual void Awake() 
    {
        cvs = GetComponent<CanvasGroup>();

        cvs.alpha = 0f;
        cvs.blocksRaycasts = false;
        cvs.interactable = false;
    }

    protected virtual void Start()
    {
        EventManager.SubGameOver(goc => Close(true));
        //EventManager.SubStartMeet(mt => Close(true));
        EventManager.SubExitRoom(() => Close(true));
    }

    public virtual void Open(bool isTweenSkip = false)
    {
        if(isTweenSkip)
        {
            cvs.alpha = 1f;
            cvs.blocksRaycasts = true;
            cvs.interactable = true;
        }
        else
        {
            if (seq != null)
            {
                seq.Kill();
            }

            seq = DOTween.Sequence();

            seq.Append(cvs.DOFade(1f, TWEEN_DURATION));
            seq.AppendCallback(() =>
            {
                cvs.blocksRaycasts = true;
                cvs.interactable = true;
            });
        }

        GameManager.Instance.IsPanelOpen = true;
        PlayerManager.Instance.Player.Move(Vector3.zero);
    }

    public virtual void Close(bool isTweenSkip = false)
    {
        if (isTweenSkip)
        {
            cvs.alpha = 0f;
            cvs.blocksRaycasts = false;
            cvs.interactable = false;
        }
        else
        {
            if (seq != null)
            {
                seq.Kill();
            }

            seq = DOTween.Sequence();

            seq.Append(cvs.DOFade(0f, TWEEN_DURATION));
            seq.AppendCallback(() =>
            {
                cvs.blocksRaycasts = false;
                cvs.interactable = false;
            });
        }

        GameManager.Instance.IsPanelOpen = false;
    }
}
