using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandBucketMObj : MonoBehaviour
{
    private Image sangImg;

    private void Awake()
    {
        sangImg = GetComponentsInChildren<Image>()[1];
    }

    public void UpdateFillAmount(float cur, float max)
    {
        sangImg.fillAmount = cur / max;
    }
}
