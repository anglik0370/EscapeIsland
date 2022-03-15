using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFillImage : MonoBehaviour
{
    private Image fillImg;

    private void Awake()
    {
        fillImg = GetComponent<Image>();
    }

    public void UpdateUI(float cur, float max)
    {
        fillImg.fillAmount = cur / max;
    }
}
