using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamVO : VO
{
    public Team team;

    public TeamVO(Team team)
    {
        this.team = team;
    }
}
