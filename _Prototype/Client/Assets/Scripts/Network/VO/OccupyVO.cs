using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OccupyVO
{
    public Area area;
    public string areaName;
    public float blueGauge;
    public float redGauge;
}

[System.Serializable]
public class OccupyListVO
{
    public List<OccupyVO> areaDataList;
}
