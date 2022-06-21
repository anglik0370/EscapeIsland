using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StorageVO
{
    public List<ItemAmount> maxAmountItemList;
    public List<ItemAmount> curAmountItemList;

    public int totalNeedItemAmount;
    public int totalCollectedItemAmount;

    public StorageVO()
    {
        maxAmountItemList = new List<ItemAmount>();
        curAmountItemList = new List<ItemAmount>();
    }
}
