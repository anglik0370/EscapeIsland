using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFillImage : MonoBehaviour
{
    private Image fillImg;

    private bool isFill;
    public bool IsFill { get => isFill; set => isFill = value; }

    private void Awake()
    {
        fillImg = GetComponent<Image>();
    }

    public void UpdateUI(float cur, float max)
    {
        if(isFill)
        {
            fillImg.fillAmount = 1f;
        }
        else
        {
            fillImg.fillAmount = cur / max;
        }
    }
}
