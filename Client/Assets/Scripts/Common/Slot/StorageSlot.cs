using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StorageSlot : ItemSlot
{
    private Text amountText;

    [SerializeField]
    private ItemSO originItem;

    public ItemSO OriginItem => originItem;

    protected override void Awake()
    {
        base.Awake();

        amountText = GetComponentInChildren<Text>();

        SetItem(originItem);
    }

    public void SetAmountText(int max, int cur)
    {
        amountText.text = $"{cur} / {max}";
    }
}
