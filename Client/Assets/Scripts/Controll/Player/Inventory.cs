using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSO> itemList = new List<ItemSO>();

    private const int MAX_ITEM_COUNT = 7;

    public void AddItem(ItemSO item)
    {
        if(itemList.Count >= MAX_ITEM_COUNT)
        {
            //만약 최대로 가질 수 있는 아이템보다 많다면 리턴

            //경고 메세지? 그런거 띄워도 될듯
            return;
        }

        //일단 지금은 리스트에만 넣어둠
        itemList.Add(item);
    }

    //이름은 Remove지만 아이템을 뽑아오는 함수다
    public ItemSO RemoveItem(int itemId)
    {
        ItemSO item = null;
        
        item = itemList.Find(x => x.itemId == itemId);

        return item;
    }
}
