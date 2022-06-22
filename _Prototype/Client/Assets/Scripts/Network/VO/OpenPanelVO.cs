using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpenPanelVO
{
    public MissionType missionType;
    public int spawnerId;
    public bool isOpen;

    public OpenPanelVO(MissionType type, int id, bool isOpen)
    {
        this.missionType = type;
        this.spawnerId = id;
        this.isOpen = isOpen;
    }
}
