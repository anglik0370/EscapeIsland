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

        //��á���� ��á�ٰ� ������ ������� �Ѵ�.
        DataVO dataVO = new DataVO("STORAGE_FULL", "");

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void AddItem(ItemSO item)
    {
        //�����ۿ� �´� AmountŬ���� ã���ְ�
        ItemAmount maxAmount = FindItemAmount(true, item);
        ItemAmount curAmount = FindItemAmount(false, item);

        //�����غ��ϱ� MAX ������ �ȵǳ� ���纸���� + �ִ¾��� ������ �����ִ°ɷ�
        if (curAmount.amount < maxAmount.amount)
        {
            //������ŭ �����ش�
            curAmount.amount++;

            totalCollectedItemAmount++;
        }

        if (IsItemFull(item))
        {
            Debug.Log($"{item}����");
        }

        if (IsItemFull())
        {
            //��á���� ��á�ٰ� ������ ������� �Ѵ�.
            DataVO dataVO = new DataVO("STORAGE_FULL", "");

            Debug.Log("����");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }

        StoragePanel.Instance.UpdateUIs(item, GetProgress());
    }

    public ItemAmount FindItemAmount(bool isMaxList, ItemSO item)
    {
        ItemAmount amount = isMaxList ? maxAmountItemList.Find(x => x.item.itemId == item.itemId) : curAmountItemList.Find(x => x.item.itemId == item.itemId);

        return amount;
    }

    //Ư�� �������� �� á���� �˻�
    public bool IsItemFull(ItemSO item)
    {
        //�����ۿ� �´� AmountŬ���� ã���ְ�
        ItemAmount maxAmount = FindItemAmount(true, item);
        ItemAmount curAmount = FindItemAmount(false, item);

        //�ΰ��� ������ ������
        return curAmount.amount == maxAmount.amount;
    }

    //�̰� ��ü�� ��á���� �˻��ϴ°�
    private bool IsItemFull()
    {
        for (int i = 0; i < curAmountItemList.Count; i++)
        {
            //�ϳ��� �ٸ��� false
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
