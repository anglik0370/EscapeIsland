using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CherryVO
{
    public Team team;
    public List<int> targetIdList;

    public CherryVO(Team team,List<int> targetIdList)
    {
        this.team = team;
        this.targetIdList = targetIdList;
    }
}
