using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageFull : ISetAble
{
    private bool needStorageFullRefresh = false;

    private string msg = string.Empty;

    void Update()
    {
        if (needStorageFullRefresh)
        {
            SetStorageFull();
            needStorageFullRefresh = false;
        }
    }
    public void SetStorageFullData(string msg)
    {
        lock (lockObj)
        {
            needStorageFullRefresh = true;
            this.msg = msg;
        }
    }

    public void SetStorageFull()
    {
        //msg¶ç¿öÁÖ±â
    }
}
