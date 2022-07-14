using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AltarPanel : Panel
{
    [SerializeField]
    private int maxItemCnt = 0;
    [SerializeField]
    private int itemCnt = 0;

    [SerializeField]
    private Transform slotParentTrm;
    private List<AltarSlot> slots = new List<AltarSlot>();

    protected override void Awake()
    {
        base.Awake();

        slots = slotParentTrm.GetComponentsInChildren<AltarSlot>().ToList();

        maxItemCnt = slots.Count;
    }

    public void DropItem()
    {
        if(itemCnt < maxItemCnt)
        {
            itemCnt++;
        }
        else
        {
            itemCnt = 0;

            //���� �ʱ�ȭ ���ְ�
            slots.ForEach(x => x.SetItem(null));

            //���⼭ ������ �ָ� ��

        }
    }
}
