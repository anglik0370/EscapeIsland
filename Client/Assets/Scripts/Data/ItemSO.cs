using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public int itemId;
    public string itemName;
    public Sprite itemSprite;

    public bool canRefining; //재련가능한 아이템인지

    //연산자 오버로딩으로 비교를 편하게 하자
    public static bool operator ==(ItemSO x, ItemSO y)
    {
        return x.itemId == y.itemId;
    }
    public static bool operator !=(ItemSO x, ItemSO y)
    {
        return x.itemId != y.itemId;
    }

    public override bool Equals(object other)
    {
        ItemSO item = other as ItemSO;

        return this.itemId == item.itemId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return itemName;
    }
}
