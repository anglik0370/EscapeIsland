using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeGaugeMObj : MonoBehaviour
{
    [SerializeField]
    private Image chargeImg;

    [SerializeField]
    private List<Sprite> chargeSpriteList;

    [SerializeField]
    private float maxTime;

    private void Start()
    {
        ChangeImage(9);
    }

    private void ChangeImage(float cur)
    {
        float curTime = cur;

        float branch = maxTime / chargeSpriteList.Count;

        int chargeCnt = 0;

        while (curTime > branch)
        {
            curTime -= branch;
            chargeCnt++;
        }

        Sprite sprite = chargeSpriteList[chargeCnt];

        if(sprite == null)
        {
            chargeImg.color = UtilClass.limpidityColor;
        }
        else
        {
            chargeImg.color = UtilClass.opacityColor;
            chargeImg.sprite = sprite;
        }
    }
}
