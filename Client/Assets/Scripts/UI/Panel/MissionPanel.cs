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
            print("이게 왜 널?");
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

        base.Open();
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
}
