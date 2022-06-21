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

    private Dictionary<Team, StorageVO> storageDic;
    public Dictionary<Team, StorageVO> StorageDic => storageDic;

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
            foreach (var team in storageDic)
            {
                team.Value.totalCollectedItemAmount = 0;
                team.Value.totalNeedItemAmount = 0;

                team.Value.maxAmountItemList.Clear();

                for (int i = 0; i < needItemSO.itemAmountList.Count; i++)
                {
                    team.Value.maxAmountItemList.Add(needItemSO.itemAmountList[i]);
                    team.Value.curAmountItemList.Add(new ItemAmount(needItemSO.itemAmountList[i].item, 0));
                }

                for (int i = 0; i < team.Value.maxAmountItemList.Count; i++)
                {
                    team.Value.totalNeedItemAmount += team.Value.maxAmountItemList[i].amount;
                }

                foreach (ItemAmount amount in team.Value.curAmountItemList)
                {
                    amount.amount = 0;

                    StoragePanel.Instance.UpdateUIs(team.Key, amount.item, GetProgress(team.Key));
                }
            }
        });
    }

    public void AddItem(Team team, ItemSO item)
    {
        //아이템에 맞는 Amount클래스 찾아주고
        ItemAmount maxAmount = FindItemAmount(true, team, item);
        ItemAmount curAmount = FindItemAmount(false, team, item);

        //생각해보니까 MAX 넘으면 안되네 현재보유량 + 넣는양이 작으면 더해주는걸로
        if (curAmount.amount < maxAmount.amount)
        {
            //갯수만큼 더해준다
            curAmount.amount++;

            if(storageDic.TryGetValue(team, out StorageVO value))
            {
                value.totalCollectedItemAmount++;
            }
        }

        if (IsItemFull(team, item))
        {
            Debug.Log($"{item}꽉참");
        }

        if (IsItemFull(team))
        {
            //꽉찼으니 꽉찼다고 서버에 보내줘야 한다.
            DataVO dataVO = new DataVO("STORAGE_FULL", "");

            Debug.Log("꽉참");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }

        StoragePanel.Instance.UpdateUIs(team, item, GetProgress(team));
    }

    public void RemoveItem(Team team, ItemSO item)
    {
        //아이템에 맞는 Amount클래스 찾아주고
        ItemAmount maxAmount = FindItemAmount(true, team, item);
        ItemAmount curAmount = FindItemAmount(false, team, item);

        //0보다 작으면 안됨
        if (curAmount.amount > 0)
        {
            //갯수만큼 빼준다
            curAmount.amount--;

            if (storageDic.TryGetValue(team, out StorageVO value))
            {
                value.totalCollectedItemAmount--;
            }
        }

        StoragePanel.Instance.UpdateUIs(team, item, GetProgress(team));
    }

    public int FindNeedItemAmount(ItemSO so)
    {
        return needItemSO.itemAmountList.Find(x => x.item == so).amount;
    }

    public ItemAmount FindItemAmount(bool isMaxList,Team team, ItemSO item)
    {
        ItemAmount amount = null;

        if(isMaxList)
        {
            if(storageDic.TryGetValue(team, out StorageVO value))
            {
                amount = value.maxAmountItemList.Find(x => x.item.itemId == item.itemId);
            }
        }
        else
        {
            if (storageDic.TryGetValue(team, out StorageVO value))
            {
                amount = value.curAmountItemList.Find(x => x.item.itemId == item.itemId);
            }
        }

        return amount;
    }

    //특정 아이템이 꽉 찼는지 검사
    public bool IsItemFull(Team team, ItemSO item)
    {
        //아이템에 맞는 Amount클래스 찾아주고
        ItemAmount maxAmount = FindItemAmount(true, team, item);
        ItemAmount curAmount = FindItemAmount(false, team, item);

        //두개가 같으면 꽉찬거
        return curAmount.amount == maxAmount.amount;
    }

    //이건 전체가 꽉찼는지 검사하는거
    private bool IsItemFull(Team team)
    {
        if(storageDic.TryGetValue(team, out StorageVO value))
        {
            for (int i = 0; i < value.curAmountItemList.Count; i++)
            {
                //하나라도 다르면 false
                if (!IsItemFull(team, value.curAmountItemList[i].item))
                {
                    return false;
                }
            }

            return true;
        }

        Debug.Log("그런 팀은 없어요");
        return false;
    }

    private float GetProgress(Team team)
    {
        if (storageDic.TryGetValue(team, out StorageVO value))
        {
            return ((float)value.totalCollectedItemAmount / (float)value.totalNeedItemAmount) * 100;
        }

        Debug.Log("그런 팀은 없어요");
        return 0;
    }

    private NeedItemSO FindNeedItemSO(int playerCnt)
    {
        return Resources.Load<NeedItemSO>($"NeedItemSO/NeedItem {playerCnt}");
    }
}
