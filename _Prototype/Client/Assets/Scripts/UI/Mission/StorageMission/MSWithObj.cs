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
            maxPanelCount = maxItemCount / objList.Count; //������ �ȳ��� ���� ��Ź

            curItemCount = 0;

            for (int i = 0; i < slotList.Count; i++)
            {
                slotList[i].DisableImg();
            }

            for (int i = 0; i < objList.Count; i++)
            {
                objList[i].Disable();
            }

            UpdateCurItem();
        });
    }

    public void Open()
    {
        UpdateCurItem();
    }

    public void Close()
    {

    }

    public void AddCurItem()
    {
        curItemCount++;

        if(maxPanelCount > 1)
        {
            int tmpItemCnt = curItemCount;

            while (tmpItemCnt > 0)
            {
                tmpItemCnt -= maxPanelCount;
            }

            if (tmpItemCnt == 0)
            {
                MissionPanel.Instance.Close();
            }
        }
        else
        {
            if(curItemCount >= maxItemCount)
            {
                MissionPanel.Instance.Close();
            }
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

        if(maxPanelCount > 1)
        {
            int itemCount = curItemCount % maxPanelCount;

            for (int i = 0; i < itemCount; i++)
            {
                slotList[i].EnableImg();
            }
        }
        else
        {
            for (int i = 0; i < curItemCount; i++)
            {
                slotList[i].EnableImg();
            }
        }
    }
}
