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

    public void SetProgress(float max, float cur)
    {
        float curTime = cur;

        float branch = max / (chargeSpriteList.Count - 1);

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
