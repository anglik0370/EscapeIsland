using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaExplanationPanel : Panel
{
    public static AreaExplanationPanel Instance;

    public bool isOpen = false;

    public Text areaNameText;
    public Text areaExplanationText;
    public Text areaItemText;

    public List<Image> itemImgList = new List<Image>();

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();
    }

    private void Start()
    {
        EventManager.SubGameOver(gos => Close(true));
    }

    public void Open(AreaSO areaSO)
    {
        if(isOpen)
        {
            return;
        }

        //초기화
        for (int i = 0; i < itemImgList.Count; i++)
        {
            itemImgList[i].gameObject.SetActive(true);
        }

        areaExplanationText.gameObject.SetActive(true);

        //열기 시작
        base.Open();
        isOpen = true;

        areaNameText.text = areaSO.areaName;
        areaExplanationText.text = areaSO.areaExplanation;

        if(areaSO.dropItemList.Count == 0)
        {
            areaItemText.gameObject.SetActive(false);
        }

        for(int i = 0; i < areaSO.dropItemList.Count; i++)
        {
            itemImgList[i].sprite = areaSO.dropItemList[i].itemSprite;
        }

        for(int i = areaSO.dropItemList.Count; i < itemImgList.Count; i++)
        {
            itemImgList[i].gameObject.SetActive(false);
        }
    }

    public override void Close(bool isTweenSkip = false)
    {
        base.Close(isTweenSkip);
        isOpen = false;
    }
}
