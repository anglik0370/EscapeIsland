using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private ItemSlot inventoryPrefab;

    public List<ItemSlot> slotList = new List<ItemSlot>();
    private List<int> itemIdList = new List<int>();

    public bool IsAllSlotFull => CheckAllSlotFull();

    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            slotList.Add(Instantiate(inventoryPrefab, transform));
        }

        EventManager.SubGameInit(() =>
        {
            CreateInventory(7); //7개로 만들어
        });

        EventManager.SubGameOver(goc =>
        {
            ClearSlots();
        });

        EventManager.SubExitRoom(() =>
        {
            ClearSlots();
        });

        EventManager.SubEnterRoom(p =>
        {
            ClearSlots();
        });
    }

    public void CreateInventory(int count)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < count; i++)
        {
            slotList[i].gameObject.SetActive(true);
        }
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

    public void RemoveItem(ItemSO item)
    {
        ItemSlot emptySlot = slotList.Find(x => !x.IsEmpty && x.GetItem().itemId == item.itemId);

        if (emptySlot != null)
        {
            emptySlot.SetItem(null);
        }
        else
        {
            Debug.Log("이 아이템이 없습니다");
        }
    }

    private void ClearSlots()
    {
        foreach (ItemSlot slot in slotList)
        {
            slot.SetItem(null);
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

    public List<int> GetItemIdList()
    {
        itemIdList.Clear();

        foreach (ItemSlot slot in slotList)
        {
            if (!slot.IsEmpty)
            {
                itemIdList.Add(slot.GetItem().itemId);
            }
        }

        return itemIdList;
    }
}
