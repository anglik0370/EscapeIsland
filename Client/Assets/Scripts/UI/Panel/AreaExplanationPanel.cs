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

    public void Open(AreaSO areaSO)
    {
        if(isOpen)
        {
            return;
        }

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
            //itemImgList[i].sprite = areaSO.dropItemList[i].itemSprite;
        }

        for(int i = areaSO.dropItemList.Count; i < itemImgList.Count; i++)
        {
            itemImgList[i].gameObject.SetActive(false);
        }
    }

    public override void Close()
    {
        base.Close();
        isOpen = false;

        for(int i = 0; i < itemImgList.Count; i++)
        {
            itemImgList[i].gameObject.SetActive(true);
        }

        areaExplanationText.gameObject.SetActive(true);
    }
}
