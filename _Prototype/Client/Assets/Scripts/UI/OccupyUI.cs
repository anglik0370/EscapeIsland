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
        UpdateUI(Team.RED);
        UpdateUI(Team.BLUE);

        DisableUI();

        EventManager.SubGameOver(goc =>
        {
            UpdateUI(Team.RED);
            UpdateUI(Team.BLUE);

            DisableUI();
        });
    }

    public void EnableUI()
    {
        UtilClass.SetCanvasGroup(cvs, 1f, false, false);
    }

    public void UpdateUI(Team team, string areaName = "", float progress = 0f)
    {
        switch (team)
        {
            case Team.RED:
                redGauge.fillAmount = progress;
                break;
            case Team.BLUE:
                blueGauge.fillAmount = progress;
                break;
        }

        areaNameTxt.text = areaName;
    }

    public void DisableUI()
    {
        UtilClass.SetCanvasGroup(cvs);
    }
}
