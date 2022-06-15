using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NeedItem", menuName = "SO/NeedItemSO")]
public class NeedItemSO : ScriptableObject
{
    public int user;
    public List<ItemAmount> itemAmountList;
}
