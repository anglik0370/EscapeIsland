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
    Battery,
    Engine,
}

public class MissionPanel : Panel
{
    [SerializeField]
    private List<IMission> missionList = new List<IMission>();

    [SerializeField]
    private Transform missionParentTrm;

    [SerializeField]
    private IMission oldMission;

    protected override void Awake()
    {
        base.Awake();

        missionList = missionParentTrm.GetComponentsInChildren<IMission>().ToList();
    }

    private void Start()
    {
        for (int i = 0; i < missionList.Count; i++)
        {
            UtilClass.SetCanvasGroup(missionList[i].Cvs);
        }

        missionList.ForEach(x => x.Init());
    }

    public void Open(MissionType type)
    {
        IMission mission = null;

        for (int i = 0; i < missionList.Count; i++)
        {
            if(missionList[i].MissionType == type)
            {
                mission = missionList[i];
                break;
            }
        }

        if(mission == null)
        {
            print("�̰� �� ��?");
            return;
        }

        if (oldMission == null)
        {
            oldMission = mission;
        }

        if (oldMission.MissionType != mission.MissionType)
        {
            UtilClass.SetCanvasGroup(oldMission.Cvs);
        }

        UtilClass.SetCanvasGroup(mission.Cvs, 1, true, true);

        oldMission = mission;

        base.Open(true);
    }

    public override void Close(bool isTweenSkip = false)
    {
        oldMission.Init();

        UtilClass.SetCanvasGroup(oldMission.Cvs);

        base.Close(isTweenSkip);
    }

    public void OpenCoconut()
    {
        Open(MissionType.Coconut);
    }

    public void OpenBerry()
    {
        Open(MissionType.Berry);
    }

    public void OpenWood()
    {
        Open(MissionType.Wood);
    }

    public void OpenOre()
    {
        Open(MissionType.Ore);
    }

    public void OpenBottle()
    {
        Open(MissionType.Bottle);
    }

    public void OpenWater()
    {
        Open(MissionType.Water);
    }

    public void OpenEngine()
    {
        Open(MissionType.Engine);
    }
}
