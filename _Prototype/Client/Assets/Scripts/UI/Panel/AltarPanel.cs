using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarPanel : Panel
{
    [SerializeField]
    private int maxItemCnt = 0;
    [SerializeField]
    private int itemCnt = 0;

    [SerializeField]
    private List<AltarSlot> slots = new List<AltarSlot>();

    protected override void Awake()
    {
        base.Awake();

        maxItemCnt = slots.Count;
    }
}
