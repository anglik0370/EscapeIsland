using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField]
    private Image gaugeImg;

    private const float MAX_AMOUNT = 100f;

    private void Start() 
    {
        UpdateProgress(0);

        EventManager.SubGameOver(goc =>
        {
            UpdateProgress(0);
        });
    }

    public void UpdateProgress(float progress)
    {
        gaugeImg.fillAmount = progress / MAX_AMOUNT;
    }
}
