using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoragePanel : Panel
{
    public static StoragePanel Instance;

    [SerializeField]
    private ProgressUI progressUI;
    [SerializeField]
    private List<StorageSlot> slotList = new List<StorageSlot>();

    private bool isGameStart = false;

    protected override void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        slotList = GetComponentsInChildren<StorageSlot>().ToList();
    }

    protected override void Start()
    {
        EventManager.SubGameStart(p =>
        {
            isGameStart = true;
        });

        EventManager.SubGameOver(goc =>
        {
            isGameStart = false;
        });

        base.Start();
    }

    public void UpdateUIs(Team team, ItemSO item, float progress)
    {
        ItemAmount maxAmount = StorageManager.Instance.FindItemAmount(true, team, item);
        ItemAmount curAmount = StorageManager.Instance.FindItemAmount(false, team, item);

        StorageSlot slot = slotList.Find(x => x.OriginItem.itemId == item.itemId);

        slot.SetAmountText(maxAmount.amount, curAmount.amount);

        progressUI.UpdateProgress(progress);
    }

    public void Open(Team team, bool isTweenSkip = false)
    {
        //Debug.LogWarning("Item을 넣어서 사용하세요");
        Open(team, null);
    }

    public void Open(Team team, ItemSO item)
    {
        if (!isGameStart) return;

        print(item + " 저장소 열기");

        base.Open();

        for(int i = 0; i < slotList.Count; i++)
        {
            ItemAmount maxAmount = StorageManager.Instance.FindItemAmount(true, team, slotList[i].OriginItem);
            ItemAmount curAmount = StorageManager.Instance.FindItemAmount(false, team, slotList[i].OriginItem);

            slotList[i].SetAmountText(maxAmount.amount, curAmount.amount);
        }
    }
}
