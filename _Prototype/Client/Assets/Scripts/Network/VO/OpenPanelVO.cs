using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpenPanelVO
{
    public Area area;
    public MissionType missionType;
    public Team team;
    public int spawnerId;
    public bool isOpen;
    public bool isGathering;

    public OpenPanelVO(Area area, MissionType type,Team team, int id, bool isOpen, bool isGathering)
    {
        this.area = area;
        this.missionType = type;
        this.team = team;
        this.spawnerId = id;
        this.isOpen = isOpen;
        this.isGathering = isGathering;
    }
}
