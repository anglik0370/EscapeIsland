using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MSGlass : MonoBehaviour, IStorageMission
{
    [SerializeField]
    private ItemSO storageItem;
    public ItemSO StorageItem => storageItem;

    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private int maxItemCount = 12; //최대 12개
    public int MaxItemCount => maxItemCount;
    [SerializeField]
    private int curItemCount = 0;
    public int CurItemCount => curItemCount;

    [SerializeField]
    private int maxPanelCount = 6; //한상자에 6개

    [SerializeField]
    private List<MSSlot> slotList;

    [SerializeField]
    private List<MSObject> barrelList = new List<MSObject>();

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        slotList = GetComponentsInChildren<MSSlot>().ToList();
    }

    private void Start()
    {
        barrelList.ForEach(x => x.Disable());
    }

    public void Open()
    {
        if (curItemCount == maxItemCount) return;

        UpdateCurItem();
    }

    public void Close()
    {
        bool isMax = true;

        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i].IsEmpty)
            {
                isMax = false;
            }
        }

        if(isMax)
        {
            slotList.ForEach(x => x.DisableImg());

            for (int i = 0; i < barrelList.Count; i++)
            {
                if (barrelList[i].IsEmpty)
                {
                    barrelList[i].Enable();
                    break;
                }
            }
        }
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

        int barrelCount = 0;

        for (int i = 0; i < curItemCount; i++)
        {
            if (maxPanelCount % i == 0)
            {
                barrelCount++;
            }
        }

        for (int i = 0; i < barrelCount; i++)
        {
            barrelList[i].Enable();
        }

        int glassCount = curItemCount % maxPanelCount;

        for (int i = 0; i < glassCount; i++)
        {
            slotList[i].EnableImg();
        }
    }
}
