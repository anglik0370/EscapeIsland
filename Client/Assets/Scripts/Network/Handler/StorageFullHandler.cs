using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageFullHandler : MonoBehaviour, IMsgHandler
{
    private StorageFull storageFull = null;
    private bool once = false;
    public void HandleMsg(string payload)
    {
        if(!once)
        {
            storageFull = NetworkManager.instance.FindSetDataScript<StorageFull>();
            once = true;
        }
        storageFull.SetStorageFullData(payload);
    }
}
