using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAndStorage : ISetAble
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
        UIManager.Instance.SetWarningText(msg);
    }

    public void SetItemDisable(int spawnerId)
    {
        ItemSpawner s = SpawnerManager.Instance.SpawnerList.Find(x => x.id == spawnerId);
        s.DeSpawnItem();
    }

    public void SetItemStorage(int itemSOId)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(itemSOId);

        StorageManager.Instance.AddItem(so);
    }
}
