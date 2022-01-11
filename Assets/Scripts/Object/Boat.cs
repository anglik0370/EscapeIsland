using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boat : MonoBehaviour
{
    public List<ItemAmount> maxAmountItemList;
    public List<ItemAmount> curAmountItemList;

    private void Awake() 
    {
        maxAmountItemList = new List<ItemAmount>();
        curAmountItemList = new List<ItemAmount>();

        //처음에 먼저 curAmountItemList를 초기화 해주자
        List<ItemSO> itemList = Resources.LoadAll<ItemSO>(typeof(ItemSO).ToString()).ToList();

        for(int i = 0; i < itemList.Count; i++)
        {
            print(itemList[i]);

            ItemAmount temp = new ItemAmount(itemList[i], 0);
            curAmountItemList.Add(temp);
        }

        {
            //여긴 디버깅 영역임
            for(int i = 0; i < itemList.Count; i++)
            {
                ItemAmount temp = new ItemAmount(itemList[i], 10);
                maxAmountItemList.Add(temp);
            }
        }
    }

    private ItemAmount FindItemAmount(List<ItemAmount> list, ItemSO item)
    {
        return list.Find(x => x.item == item);
    }

    public void AddItem(ItemSO item)
    {
        //아이템에 맞는 Amount클래스 찾아주고
        ItemAmount curItemAmount = FindItemAmount(curAmountItemList, item);
        ItemAmount maxItemAmount = FindItemAmount(maxAmountItemList, item);

        //생각해보니까 MAX 넘으면 안되네 현재보유량 + 넣는양이 작으면 더해주는걸로
        if(curItemAmount.amount <= maxItemAmount.amount)
        {
            //갯수만큼 더해준다
            curItemAmount.amount++;
        }
    }

    //특정 아이템이 꽉 찼는지 검사
    public bool IsItemFull(ItemSO item)
    {
        //아이템에 맞는 Amount클래스 찾아주고
        ItemAmount curItemAmount = FindItemAmount(curAmountItemList, item);
        ItemAmount maxItemAmount = FindItemAmount(maxAmountItemList, item);

        //두개가 같으면 꽉찬거
        return curItemAmount.amount == maxItemAmount.amount;
    }

    //이건 전체가 꽉찼는지 검사하는거
    public bool IsItemFull()
    {
        for(int i = 0; i < curAmountItemList.Count; i++)
        {
            //하나라도 다르면 false
            if(!IsItemFull(curAmountItemList[i].item))
            {
                return false;
            }
        }    

        return true;
    }
}
