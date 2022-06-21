using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MissionType
{
    Wood,
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
    private List<IGetMission> getMissionList = new List<IGetMission>();
    private List<IStorageMission> storageMissionList = new List<IStorageMission>();

    [SerializeField]
    private List<MissionType> notCoolTimeMissionList = new List<MissionType>();

    [SerializeField]
    private Transform missionParentTrm;

    [SerializeField]
    private IMission oldMission;

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

    public void OpenGetMission(MissionType type, ItemCharger charger = null)
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

        UtilClass.SetCanvasGroup(getMission.Cvs, 1, true, true);

        getMission.Open();

        oldMission = getMission;

        base.Open(true);
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

    public override void Close(bool isTweenSkip = false)
    {
        if (oldMission == null) return;

        oldMission?.Close();

        UtilClass.SetCanvasGroup(oldMission.Cvs);

        base.Close(isTweenSkip);
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

    public IStorageMission FindStorageMissionByItemId(int itemId)
    {
        return storageMissionList.Find(x => x.StorageItem.itemId.Equals(itemId));
    }

    public IMission FindMissionByType(MissionType type)
    {
        return getMissionList.Find(x => x.MissionType.Equals(type));
    }

    public void OpenCoconut()
    {
        OpenGetMission(MissionType.Coconut);
    }

    public void OpenBerry()
    {
        OpenGetMission(MissionType.Berry);
    }

    public void OpenWood()
    {
        OpenGetMission(MissionType.Wood);
    }

    public void OpenOre()
    {
        OpenGetMission(MissionType.Ore);
    }

    public void OpenBottle()
    {
        OpenGetMission(MissionType.Bottle);
    }

    public void OpenWater()
    {
        OpenGetMission(MissionType.Water);
    }

    public void OpenEngine()
    {
        OpenGetMission(MissionType.Engine);
    }

    public void OpenCharge()
    {
        OpenGetMission(MissionType.Charge);
    }

    public void OpenBattery()
    {
        OpenGetMission(MissionType.Battery);
    }

    public void OpenSand()
    {
        OpenGetMission(MissionType.Sand);
    }
}
