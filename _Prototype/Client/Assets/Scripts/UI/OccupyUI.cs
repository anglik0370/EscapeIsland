using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OccupyUI : MonoBehaviour
{
    private CanvasGroup cvs;

    [SerializeField]
    private Image redGauge;
    [SerializeField]
    private Image blueGauge;

    [SerializeField]
    private Text areaNameTxt;

    private Sequence seq;
    private float duration = 0.2f;

    public bool IsOpen => cvs.alpha == 1;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        seq = DOTween.Sequence();
    }

    private void Start()
    {
        SetUI();

        DisableUI();

        EventManager.SubGameOver(goc =>
        {
            SetUI();

            DisableUI();
        });

        EventManager.SubExitRoom(() =>
        {
            SetUI();

            DisableUI();
        });

    }

    public void EnableUI()
    {
        UtilClass.SetCanvasGroup(cvs, 1f, false, false);
    }

    public void SetUI(float redProgress = 0f, float blueProgress = 0f, string areaName = "")
    {
        redGauge.fillAmount = redProgress;
        blueGauge.fillAmount = blueProgress;
        areaNameTxt.text = areaName;
    }

    public void UpdateUI(float redProgress = 0f, float blueProgress = 0f)
    {
        if(redGauge.fillAmount >= 1f || blueGauge.fillAmount >= 1f)
        {
            DisableUI();
            return;
        }

        if(seq != null)
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => redGauge.fillAmount, x => redGauge.fillAmount = x, redProgress, duration));
        seq.Join(DOTween.To(() => blueGauge.fillAmount, x => blueGauge.fillAmount = x, blueProgress, duration).OnComplete(() => 
        {
            if(redProgress >= 1f || blueProgress >= 1f)
            {
                DisableUI();
            }
        }));
    }

    public void DisableUI()
    {
        UtilClass.SetCanvasGroup(cvs);
    }
}
