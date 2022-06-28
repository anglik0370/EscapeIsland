using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpenPanelVO
{
    public MissionType missionType;
    public Team team;
    public int spawnerId;
    public bool isOpen;

    public OpenPanelVO(MissionType type,Team team, int id, bool isOpen)
    {
        this.missionType = type;
        this.team = team;
        this.spawnerId = id;
        this.isOpen = isOpen;
    }
}
