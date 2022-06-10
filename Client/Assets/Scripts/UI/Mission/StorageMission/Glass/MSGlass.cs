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
    private List<MSGlassSlot> slotList;

    [SerializeField]
    private List<MSObject> barrelList = new List<MSObject>();

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        slotList = GetComponentsInChildren<MSGlassSlot>().ToList();
    }

    private void Start()
    {
        barrelList.ForEach(x => x.Disable());
    }

    public void Open()
    {
        bool isMax = true;

        for (int i = 0; i < barrelList.Count; i++)
        {
            if (barrelList[i].IsEmpty)
            {
                isMax = false;
                break;
            }
        }

        if (isMax) return;
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
}
