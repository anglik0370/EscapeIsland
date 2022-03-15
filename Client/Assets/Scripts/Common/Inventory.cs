using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> slotList = new List<ItemSlot>();

    public bool IsAllSlotFull => CheckAllSlotFull();

    private void Awake() 
    {
        slotList = GetComponentsInChildren<ItemSlot>().ToList();
    }

    private void Start()
    {
        EventManager.SubGameOver(goc =>
        {
            foreach (ItemSlot slot in slotList)
            {
                slot.SetItem(null);
            }
        });
    }

    //생각해보니까 그냥 넣는함수만 있어도 되지않나 넣는거 뺴고는 나머지 다 드래그앤드랍이니까
    public void AddItem(ItemSO item)
    {
        //빈 슬롯을 찾자
        ItemSlot emptySlot = slotList.Find(x => x.IsEmpty);

        if(emptySlot != null)
        {
            emptySlot.SetItem(item);
        }
        else
        {
            Debug.Log("인벤토리가 꽉찼습니다");
        }
    }

    private bool CheckAllSlotFull()
    {
        foreach(ItemSlot slot in slotList)
        {
            if(slot.IsEmpty)
            {
                return false;
            }
        }

        return true;
    }
}
