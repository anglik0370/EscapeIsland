using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MSOnly : MonoBehaviour, IStorageMission
{
    [SerializeField]
    private ItemSO storageItem;
    public ItemSO StorageItem => storageItem;

    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private int maxItemCount;
    public int MaxItemCount => maxItemCount;

    [SerializeField]
    private int curItemCount;
    public int CurItemCount => curItemCount;

    [SerializeField]
    private List<MSSlot> slotList;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        slotList = GetComponentsInChildren<MSSlot>().ToList();
    }

    private void Start()
    {
        EventManager.SubGameStart(p =>
        {
            maxItemCount = StorageManager.Instance.FindNeedItemAmount(storageItem);
        });
    }

    public void Close()
    {
        
    }

    public void Open()
    {
        if (curItemCount >= maxItemCount) return;

        UpdateCurItem();
    }

    public void AddCurItem()
    {
        curItemCount++;
    }

    public void UpdateCurItem()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].DisableImg();
        }

        for (int i = 0; i < curItemCount; i++)
        {
            slotList[i].EnableImg();
        }
    }
}
