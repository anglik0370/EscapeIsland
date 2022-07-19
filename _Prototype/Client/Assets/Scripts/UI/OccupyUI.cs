using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OccupyUI : MonoBehaviour
{
    private CanvasGroup cvs;

    [SerializeField]
    private Image redGauge;
    [SerializeField]
    private Image blueGauge;

    [SerializeField]
    private Text areaNameTxt;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        SetUI();
        SetUI();

        DisableUI();

        EventManager.SubGameOver(goc =>
        {
            SetUI();
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

    public void DisableUI()
    {
        UtilClass.SetCanvasGroup(cvs);
    }
}
