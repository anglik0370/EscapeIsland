using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; private set; }

    private ItemStorage storage;
    public ItemStorage Storage => storage;

    [SerializeField]
    private NeedItemSO needItemSO;

    [SerializeField]
    private List<ItemAmount> maxAmountItemList;
    [SerializeField]
    private List<ItemAmount> curAmountItemList;

    private int totalNeedItemAmount;
    private int totalCollectedItemAmount;

    [SerializeField]
    private ItemStorage curOpenStorage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            GameManager.Instance.AddInteractionObj(storage);
        });

        EventManager.SubGameStart(p =>
        {
            totalCollectedItemAmount = 0;
            totalNeedItemAmount = 0;

            maxAmountItemList.Clear();

            for (int i = 0; i < needItemSO.itemAmountList.Count; i++)
            {
                maxAmountItemList.Add(needItemSO.itemAmountList[i]);
                curAmountItemList.Add(new ItemAmount(needItemSO.itemAmountList[i].item, 0));
            }

            for (int i = 0; i < maxAmountItemList.Count; i++)
            {
                totalNeedItemAmount += maxAmountItemList[i].amount;
            }

            foreach (ItemAmount amount in curAmountItemList)
            {
                amount.amount = 0;

                StoragePanel.Instance.UpdateUIs(amount.item, GetProgress());
            }
        });
    }

    public void FillAllItem()
    {
        for (int i = 0; i < maxAmountItemList.Count; i++)
        {
            ItemAmount curAmount = FindItemAmount(false, maxAmountItemList[i].item);

            curAmount.amount = maxAmountItemList[i].amount;

            StoragePanel.Instance.UpdateUIs(maxAmountItemList[i].item, GetProgress());
        }

        totalNeedItemAmount = totalCollectedItemAmount;

        //???????? ???????? ?????? ???????? ????.
        DataVO dataVO = new DataVO("STORAGE_FULL", "");

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void AddItem(ItemSO item)
    {
        //???????? ???? Amount?????? ????????
        ItemAmount maxAmount = FindItemAmount(true, item);
        ItemAmount curAmount = FindItemAmount(false, item);

        //???????????? MAX ?????? ?????? ?????????? + ???????? ?????? ????????????
        if (curAmount.amount < maxAmount.amount)
        {
            //???????? ????????
            curAmount.amount++;

            totalCollectedItemAmount++;
        }

        if (IsItemFull(item))
        {
            Debug.Log($"{item}????");
        }

        if (IsItemFull())
        {
            //???????? ???????? ?????? ???????? ????.
            DataVO dataVO = new DataVO("STORAGE_FULL", "");

            Debug.Log("????");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }

        StoragePanel.Instance.UpdateUIs(item, GetProgress());
    }

    public void RemoveItem(ItemSO item)
    {
        //???????? ???? Amount?????? ????????
        ItemAmount maxAmount = FindItemAmount(true, item);
        ItemAmount curAmount = FindItemAmount(false, item);

        //0???? ?????? ????
        if (curAmount.amount > 0)
        {
            //???????? ??????
            curAmount.amount--;

            totalCollectedItemAmount--;
        }

        StoragePanel.Instance.UpdateUIs(item, GetProgress());
    }

    public int FindNeedItemAmount(ItemSO so)
    {
        return needItemSO.itemAmountList.Find(x => x.item == so).amount;
    }

    public ItemAmount FindItemAmount(bool isMaxList, ItemSO item)
    {
        ItemAmount amount = isMaxList ? maxAmountItemList.Find(x => x.item.itemId == item.itemId) : curAmountItemList.Find(x => x.item.itemId == item.itemId);

        return amount;
    }

    //???? ???????? ?? ?????? ????
    public bool IsItemFull(ItemSO item)
    {
        //???????? ???? Amount?????? ????????
        ItemAmount maxAmount = FindItemAmount(true, item);
        ItemAmount curAmount = FindItemAmount(false, item);

        //?????? ?????? ??????
        return curAmount.amount == maxAmount.amount;
    }

    //???? ?????? ???????? ??????????
    private bool IsItemFull()
    {
        for (int i = 0; i < curAmountItemList.Count; i++)
        {
            //???????? ?????? false
            if (!IsItemFull(curAmountItemList[i].item))
            {
                return false;
            }
        }

        return true;
    }

    private float GetProgress()
    {
        return ((float)totalCollectedItemAmount / (float)totalNeedItemAmount) * 100;
    }

    private NeedItemSO FindNeedItemSO(int playerCnt)
    {
        return Resources.Load<NeedItemSO>($"NeedItemSO/NeedItem {playerCnt}");
    }
}
