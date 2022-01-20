using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    private Text percentText;

    private void Awake() 
    {
        percentText = GameObject.Find("Text").GetComponent<Text>();
    }

    public void UpdateProgress(float progress)
    {
        percentText.text = $"{Mathf.RoundToInt(progress)}%";
    }
}
