using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OccupyVO : VO
{
    public Area area;
    public string areaName;
    public float blueGauge;
    public float redGauge;
    public Team occupyTeam;
}

[System.Serializable]
public class OccupyListVO : VO
{
    public bool isOpen;
    public List<OccupyVO> areaDataList;
}
