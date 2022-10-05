using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MissionType
{
    Wood = 0,
    Coconut,
    Berry,
    Ore,
    Bottle,
    Water,
    Charge,
    Engine,
    Battery,
    Sand,
    TrashCan,
    None,
}

public class MissionPanel : Panel
{
    public static MissionPanel Instance { get; private set; }

    [SerializeField]
    private List<ItemSO> missionItemList = new List<ItemSO>();
    public List<ItemSO> MissionItemList => missionItemList;

    [SerializeField]
    private List<IGetMission> getMissionList = new List<IGetMission>();
    private List<IStorageMission> storageMissionList = new List<IStorageMission>();

    [SerializeField]
    private List<MissionType> notCoolTimeMissionList = new List<MissionType>();

    [SerializeField]
    private Transform missionParentTrm;

    [SerializeField]
    private IMission oldMission;

    private ItemSpawner oldSpawner = null;

    private bool isGetMissionPanelOpen = false;
    public bool IsGetMissionPanelOpen => isGetMissionPanelOpen;

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        getMissionList = missionParentTrm.GetComponentsInChildren<IGetMission>().ToList();
        storageMissionList = missionParentTrm.GetComponentsInChildren<IStorageMission>().ToList();
    }

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < getMissionList.Count; i++)
        {
            UtilClass.SetCanvasGroup(getMissionList[i].Cvs);
        }

        for (int i = 0; i < storageMissionList.Count; i++)
        {
            UtilClass.SetCanvasGroup(storageMissionList[i].Cvs);
        }

        getMissionList.ForEach(x => x.Close());
        storageMissionList.ForEach(x => x.Close());
    }

    public void OpenGetMission(MissionType type, ItemCharger charger = null, ItemSpawner spawner = null)
    {
        IGetMission getMission = null;

        for (int i = 0; i < getMissionList.Count; i++)
        {
            if(getMissionList[i].MissionType == type)
            {
                getMission = getMissionList[i];
                break;
            }
        }

        if(getMission == null)
        {
            print("이게 왜 널?");
            return;
        }

        if (oldMission != null)
        {
            UtilClass.SetCanvasGroup(oldMission.Cvs);
        }

        if (getMission.MissionType == MissionType.Charge)
        {
            MissionCharge missionCharge = getMission as MissionCharge;

            if(charger == null)
            {
                print("차저를 넣어야지 신아");
            }

            missionCharge.SetCurCharger(charger);
        }

        oldMission = getMission;
        oldSpawner = spawner;

        if (spawner != null && NeedCoolTimeMission(type))
        {
            spawner.SetOpen(true);
        }
        else if(spawner != null)
        {
            OpenMissionPanel();
        }

        //UtilClass.SetCanvasGroup(getMission.Cvs, 1, true, true);

        //getMission.Open();

        //Open(true);
    }

    public void OpenMissionPanel()
    {
        if (oldMission == null) return;

        UtilClass.SetCanvasGroup(oldMission.Cvs, 1, true, true);

        oldMission.Open();

        Open(true);
    }

    public void OpenStorageMission(Team team, ItemSO itemSO)
    {
        IStorageMission storageMission = null;

        for (int i = 0; i < storageMissionList.Count; i++)
        {
            if(storageMissionList[i].StorageItem == itemSO)
            {
                storageMission = storageMissionList[i];
                break;
            }
        }

        if(storageMission == null)
        {
            print("아직 안만든 미션임");
            return;
        }

        int curItemCount = StorageManager.Instance.FindItemAmount(false, team, storageMission.StorageItem).amount;
        int maxItemCount = StorageManager.Instance.FindItemAmount(true, team, storageMission.StorageItem).amount;

        if (curItemCount >= maxItemCount) return;

        if (oldMission != null)
        {
            UtilClass.SetCanvasGroup(oldMission.Cvs);
        }

        UtilClass.SetCanvasGroup(storageMission.Cvs, 1, true, true);

        storageMission.SetTeam(team);
        storageMission.Open();

        oldMission = storageMission;

        base.Open(true);
    }

    public override void Open(bool isTweenSkip = false)
    {
        isGetMissionPanelOpen = true;

        base.Open(isTweenSkip);
    }

    public override void Close(bool isTweenSkip = false)
    {
        if (oldMission == null) return;

        if(oldSpawner != null && NeedCoolTimeMission(oldSpawner.MissionType))
        {
            oldSpawner.SetOpen(false);
        }

        isGetMissionPanelOpen = false;
        oldMission?.Close();

        UtilClass.SetCanvasGroup(oldMission.Cvs);

        base.Close(isTweenSkip);
    }

    public void CloseGetMissionPanel()
    {
        if (!isGetMissionPanelOpen) return;

        Close();
    }

    public bool NeedCoolTimeMission(MissionType type)
    {
        for (int i = 0; i < notCoolTimeMissionList.Count; i++)
        {
            if(notCoolTimeMissionList[i].Equals(type))
            {
                return false;
            }
        }

        return true;
    }

    public IStorageMission FindStorageMissionByItemId(int itemId, Team team)
    {
        return storageMissionList.Find(x => x.StorageItem.itemId.Equals(itemId) && x.Team.Equals(team));
    }

    public IMission FindMissionByType(MissionType type)
    {
        return getMissionList.Find(x => x.MissionType.Equals(type));
    }
}
