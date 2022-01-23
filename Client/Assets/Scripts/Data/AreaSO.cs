using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AreaSO", fileName = "New AreaSO")]
public class AreaSO : ScriptableObject
{
    public string areaName;
    public string areaExplanation;

    //해당 구역에서 얻을 수 있는 아이템 리스트
    public List<ItemSO> dropItemList;
}
