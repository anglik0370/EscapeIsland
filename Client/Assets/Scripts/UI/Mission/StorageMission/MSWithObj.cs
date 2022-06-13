using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MSWithObj : MonoBehaviour, IStorageMission
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
    private int maxPanelCount;

    [SerializeField]
    private List<MSSlot> slotList;

    [SerializeField]
    private List<MSObject> objList = new List<MSObject>();

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        slotList = GetComponentsInChildren<MSSlot>().ToList();
    }

    private void Start()
    {
        objList.ForEach(x => x.Disable());

        EventManager.SubGameStart(p =>
        {
            maxItemCount = StorageManager.Instance.FindNeedItemAmount(storageItem);
            maxPanelCount = maxItemCount / objList.Count; //나머지 안남게 세팅 부탁
        });
    }

    public void Open()
    {
        if (curItemCount >= maxItemCount) return;

        UpdateCurItem();
    }

    public void Close()
    {

    }

    public void AddCurItem()
    {
        curItemCount++;

        int tmpItemCnt = curItemCount;

        while (tmpItemCnt > 0)
        {
            tmpItemCnt -= maxPanelCount;
        }

        if(tmpItemCnt == 0)
        {
            MissionPanel.Instance.Close();
        }
    }

    public void UpdateCurItem()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].DisableImg();
        }

        int objCount = 0;

        for (int i = curItemCount - maxPanelCount; i >= 0; i -= maxPanelCount)
        {
            objCount++;
        }

        if(objCount > 0)
        {
            for (int i = 0; i < objCount; i++)
            {
                objList[i].Enable();
            }
        }

        int itemCount = curItemCount % maxPanelCount;

        for (int i = 0; i < itemCount; i++)
        {
            slotList[i].EnableImg();
        }
    }
}
