using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : ISetAble
{
    public void SetStartConvert(int converterId, int itemSOId)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(itemSOId);

        Debug.Log($"변환기{converterId}에서 {so}변환 시작");

        GameManager.Instance.refineryList.Find(x => x.id == converterId).ConvertingStart(so);

        print("start");
    }

    public void SetResetConverter(int converterId)
    {
        ItemConverter converter = GameManager.Instance.refineryList.Find(x => x.id == converterId);
        converter.ConvertingReset();
        print("reset");
    }

    public void SetTakeConverterAfterItem(int converterId)
    {
        ItemConverter converter = GameManager.Instance.refineryList.Find(x => x.id == converterId);
        converter.TakeIAfterItem();
        //refinery.ingotItem = null;
        print("take");
    }
}
