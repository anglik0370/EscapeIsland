using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSGlass : MonoBehaviour, IStorageMission
{
    [SerializeField]
    private ItemSO storageItem;
    public ItemSO StorageItem => storageItem;

    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    public void Open()
    {
        
    }

    public void Close()
    {
        
    }
}
