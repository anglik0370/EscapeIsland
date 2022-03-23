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

    public List<ItemAmount> MaxAmountItemList => maxAmountItemList;
    public List<ItemAmount> CurAmountItemList => curAmountItemList;

    private int totalNeedItemAmount;
    private int totalCollectedItemAmount;

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

        EventManager.SubGameStart(p =>
        {
            totalCollectedItemAmount = 0;
            totalNeedItemAmount = 0;

            for (int i = 0; i < maxAmountItemList.Count; i++)
            {
                maxAmountItemList[i].amount = needItemSO.itemAmountList[i].amount;
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
    public bool IsItemFull()
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

    public float GetProgress()
    {
        return ((float)totalCollectedItemAmount / (float)totalNeedItemAmount) * 100;
    }
}
