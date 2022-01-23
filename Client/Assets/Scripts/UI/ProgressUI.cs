using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    private Text percentText;
    private Image gaugeImg;

    private const float MAX_AMOUNT = 100f;

    private void Awake() 
    {
        percentText = GameObject.Find("Text").GetComponent<Text>();
        gaugeImg = GameObject.Find("Gauge").GetComponent<Image>();
    }

    public void UpdateProgress(float progress)
    {
        percentText.text = $"{Mathf.RoundToInt(progress)}%";
        gaugeImg.fillAmount = progress / MAX_AMOUNT;
    }
}
