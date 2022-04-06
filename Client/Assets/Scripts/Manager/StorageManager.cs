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

    private Player player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        storage = FindObjectOfType<ItemStorage>();
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            GameManager.Instance.AddInteractionObj(storage);
        });

        EventManager.SubGameStart(p =>
        {
            player = p;

            totalCollectedItemAmount = 0;
            totalNeedItemAmount = 0;

            maxAmountItemList.Clear();

            needItemSO = FindNeedItemSO(PlayerManager.Instance.PlayerList.Count + 1);

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

        //꽉찼으니 꽉찼다고 서버에 보내줘야 한다.
        DataVO dataVO = new DataVO("STORAGE_FULL", "");

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void AddItem(ItemSO item)
    {
        //아이템에 맞는 Amount클래스 찾아주고
        ItemAmount maxAmount = FindItemAmount(true, item);
        ItemAmount curAmount = FindItemAmount(false, item);

        //생각해보니까 MAX 넘으면 안되네 현재보유량 + 넣는양이 작으면 더해주는걸로
        if (curAmount.amount < maxAmount.amount)
        {
            //갯수만큼 더해준다
            curAmount.amount++;

            totalCollectedItemAmount++;
        }

        if (IsItemFull(item))
        {
            Debug.Log($"{item}꽉참");
        }

        if (IsItemFull())
        {
            //꽉찼으니 꽉찼다고 서버에 보내줘야 한다.
            DataVO dataVO = new DataVO("STORAGE_FULL", "");

            Debug.Log("꽉참");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }

        StoragePanel.Instance.UpdateUIs(item, GetProgress());
    }

    public ItemAmount FindItemAmount(bool isMaxList, ItemSO item)
    {
        ItemAmount amount = isMaxList ? maxAmountItemList.Find(x => x.item.itemId == item.itemId) : curAmountItemList.Find(x => x.item.itemId == item.itemId);

        return amount;
    }

    //특정 아이템이 꽉 찼는지 검사
    public bool IsItemFull(ItemSO item)
    {
        //아이템에 맞는 Amount클래스 찾아주고
        ItemAmount maxAmount = FindItemAmount(true, item);
        ItemAmount curAmount = FindItemAmount(false, item);

        //두개가 같으면 꽉찬거
        return curAmount.amount == maxAmount.amount;
    }

    //이건 전체가 꽉찼는지 검사하는거
    private bool IsItemFull()
    {
        for (int i = 0; i < curAmountItemList.Count; i++)
        {
            //하나라도 다르면 false
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
