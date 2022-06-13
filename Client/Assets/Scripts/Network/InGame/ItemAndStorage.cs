using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAndStorage : ISetAble
{
    public static ItemAndStorage Instance { get; private set; }

    private bool needStorageFullRefresh = false;
    private bool needSetMissionCool = false;

    private string msg = string.Empty;
    private ItemSpawnerVO missionData = null;

    private void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        if (needStorageFullRefresh)
        {
            SetStorageFull();
            needStorageFullRefresh = false;
        }
        if(needSetMissionCool)
        {
            SetMissionCoolTime();
            needSetMissionCool = false;
        }
    }
    public static void SetStorageFullData(string msg)
    {
        lock (Instance.lockObj)
        {
            Instance.needStorageFullRefresh = true;
            Instance.msg = msg;
        }
    }
    public static void SetMissionCool(ItemSpawnerVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.missionData = vo;
            Instance.needSetMissionCool = true;
        }
    }

    public void SetStorageFull()
    {
        //msg¶ç¿öÁÖ±â
        UIManager.Instance.AlertText(msg, AlertType.GameEvent);
    }

    public void SetMissionCoolTime()
    {
        ItemSpawner s = SpawnerManager.Instance.SpawnerList.Find(x => x.id == missionData.spawnerId && x.MissionType == missionData.missionType);
        //s.DeSpawnItem();
        if(s != null)
        {
            s.StartTimer();
        }
    }

    public void SetItemStorage(int itemSOId)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(itemSOId);
        IStorageMission mission = MissionPanel.Instance.FindStorageMissionByItemId(itemSOId);

        mission.AddCurItem();
        mission.UpdateCurItem();
        StorageManager.Instance.AddItem(so);
    }
}
