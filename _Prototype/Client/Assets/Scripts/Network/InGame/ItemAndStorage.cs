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
        //msg?????ֱ?
        UIManager.Instance.AlertText(msg, AlertType.GameEvent);
    }

    public void SetMissionCoolTime()
    {
        ItemSpawner s = SpawnerManager.Instance.FindSpawner(missionData.spawnerId, missionData.missionType);
        //s.DeSpawnItem();
        if (s != null)
        {
            if (NetworkManager.instance.socketId.Equals(missionData.senderId))
            {
                s.StartTimer(user.CoolTimeMagnification);
            }
            else if (NetworkManager.instance.GetPlayerDic().TryGetValue(missionData.senderId, out Player p))
            {
                s.StartTimer(p.CoolTimeMagnification);
            }
        }
    }

    public void SetItemStorage(ItemStorageVO vo)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(vo.itemSOId);
        IStorageMission mission = MissionPanel.Instance.FindStorageMissionByItemId(vo.itemSOId);

        StorageManager.Instance.AddItem(vo.team, so);
        mission.UpdateCurItem();
    }
}
