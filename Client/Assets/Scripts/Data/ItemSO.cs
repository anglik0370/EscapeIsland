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

    public override string ToString()
    {
        return itemName;
    }
}
