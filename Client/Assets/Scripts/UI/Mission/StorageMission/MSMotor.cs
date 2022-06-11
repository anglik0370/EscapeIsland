using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MSMotor : MonoBehaviour, IStorageMission
{
    [SerializeField]
    private ItemSO storageItem;
    public ItemSO StorageItem => storageItem;

    [SerializeField]
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    public int MaxItemCount => throw new System.NotImplementedException();

    public int CurItemCount => throw new System.NotImplementedException();

    [SerializeField]
    private List<MSSlot> slotList;

    private void Awake()
    {
        slotList = GetComponentsInChildren<MSSlot>().ToList();
    }

    public void Close()
    {
        
    }

    public void Open()
    {
        
    }

    public void AddCurItem()
    {
        
    }

    public void UpdateCurItem()
    {

    }
}
