using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottleGhostMObj : MonoBehaviour
{
    private RectTransform rect;

    private CanvasGroup cvs;

    private Image bottleImg;
    private Image waterImg;

    public RectTransform BottleRect => bottleImg.GetComponent<RectTransform>();

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        cvs = GetComponent<CanvasGroup>();

        bottleImg = transform.Find("BottleImg").GetComponent<Image>();
        waterImg = transform.Find("WaterImg").GetComponent<Image>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        waterImg.fillAmount = 0f;
    }

    public void SetPosition(Vector2 pos)
    {
        rect.position = pos;
    }

    public void SetWaterProgress(float progress)
    {
        waterImg.fillAmount = progress;
    }

    public void Enable()
    {
        UtilClass.SetCanvasGroup(cvs, 1f, true, true);
    }

    public void Disable()
    {
        UtilClass.SetCanvasGroup(cvs);
    }
}
