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
    private SerializableDictionary<Team, StorageVO> storageDic;
    public SerializableDictionary<Team, StorageVO> StorageDic => storageDic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        storageDic = new SerializableDictionary<Team, StorageVO>() { { Team.RED, new StorageVO() }, { Team.BLUE, new StorageVO() } };
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            GameManager.Instance.AddInteractionObj(storage);
        });

        EventManager.SubGameInit(() =>
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
        //�����ۿ� �´� AmountŬ���� ã���ְ�
        ItemAmount maxAmount = FindItemAmount(true, team, item);
        ItemAmount curAmount = FindItemAmount(false, team, item);

        //�����غ��ϱ� MAX ������ �ȵǳ� ���纸���� + �ִ¾��� ������ �����ִ°ɷ�
        if (curAmount.amount < maxAmount.amount)
        {
            //������ŭ �����ش�
            curAmount.amount++;

            if(storageDic.TryGetValue(team, out StorageVO value))
            {
                value.totalCollectedItemAmount++;
            }
        }

        //if (IsItemFull(team, item))
        //{
        //    Debug.Log($"{item}����");
        //}

        StoragePanel.Instance.UpdateUIs(team, item, GetProgress(team));
    }

    public void RemoveItem(Team team, ItemSO item)
    {
        //�����ۿ� �´� AmountŬ���� ã���ְ�
        ItemAmount maxAmount = FindItemAmount(true, team, item);
        ItemAmount curAmount = FindItemAmount(false, team, item);

        //0���� ������ �ȵ�
        if (curAmount.amount > 0)
        {
            //������ŭ ���ش�
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

        if(storageDic.TryGetValue(team, out StorageVO value))
        {
            if (isMaxList)
            {
                amount = value.maxAmountItemList.Find(x => x.item.itemId == item.itemId);
            }
            else
            {
                amount = value.curAmountItemList.Find(x => x.item.itemId == item.itemId);
            }
        }

        return amount;
    }

    //Ư�� �������� �� á���� �˻�
    public bool IsItemFull(Team team, ItemSO item)
    {
        //�����ۿ� �´� AmountŬ���� ã���ְ�
        ItemAmount maxAmount = FindItemAmount(true, team, item);
        ItemAmount curAmount = FindItemAmount(false, team, item);

        //�ΰ��� ������ ������
        return curAmount.amount == maxAmount.amount;
    }

    public bool IsItemFull(Team team, ItemSO item,object data)
    {
        ItemAmount maxAmount = FindItemAmount(true, team, item);
        ItemAmount curAmount = FindItemAmount(false, team, item);

        return (curAmount.amount + 1) == maxAmount.amount || curAmount.amount == maxAmount.amount;
    }

    //�̰� ��ü�� ��á���� �˻��ϴ°�
    public bool IsItemFull(Team team)
    {
        if(storageDic.TryGetValue(team, out StorageVO value))
        {
            for (int i = 0; i < value.curAmountItemList.Count; i++)
            {
                //�ϳ��� �ٸ��� false
                if (!IsItemFull(team, value.curAmountItemList[i].item,null))
                {
                    return false;
                }
            }

            return true;
        }

        Debug.Log("�׷� ���� �����");
        return false;
    }

    private float GetProgress(Team team)
    {
        if (storageDic.TryGetValue(team, out StorageVO value))
        {
            return ((float)value.totalCollectedItemAmount / (float)value.totalNeedItemAmount) * 100;
        }

        Debug.Log("�׷� ���� �����");
        return 0;
    }

    private NeedItemSO FindNeedItemSO(int playerCnt)
    {
        return Resources.Load<NeedItemSO>($"NeedItemSO/NeedItem {playerCnt}");
    }
}
