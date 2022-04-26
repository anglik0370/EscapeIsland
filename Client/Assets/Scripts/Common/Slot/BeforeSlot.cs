using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeforeSlot : ItemSlot
{
    private ItemConverter converter;
    public ItemConverter ConverterObj => converter;

    protected override void Awake()
    {
        converter = GetComponentInParent<ItemConverter>();

        base.Awake();
    }
}
