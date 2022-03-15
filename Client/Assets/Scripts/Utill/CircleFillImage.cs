using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFillImage : MonoBehaviour
{
    private Image fillImg;

    [SerializeField]
    private Color coolColor; //정상적인 쿨타임 색
    [SerializeField]
    private Color cantColor; //투표 끝나고 색

    private void Awake()
    {
        fillImg = GetComponent<Image>();
    }

    private void Start()
    {
        EventManager.SubGameOver(goc =>
        {
            SetColor(true);
        });
    }

    public void UpdateUI(float cur, float max)
    {
        fillImg.fillAmount = cur / max;
    }

    public void SetColor(bool can)
    {
        if(can)
        {
            fillImg.color = coolColor;
        }
        else
        {
            fillImg.color = cantColor;
        }
    }
}
