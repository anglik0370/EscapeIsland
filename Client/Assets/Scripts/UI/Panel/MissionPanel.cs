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

    public void Open(MissionType type)
    {

    }

    private void SetMission(IMission mission)
    {
        if(oldMission == null)
        {
            oldMission = mission;
        }

        if(oldMission.MissionType != mission.MissionType)
        {
            for (int i = 0; i < missionList.Count; i++)
            {
                missionList[i].Cvs.alpha = 0;
                missionList[i].Cvs.blocksRaycasts = false;
                missionList[i].Cvs.interactable = false;
            }
        }

        mission.Cvs.alpha = 1f;
    }
}
