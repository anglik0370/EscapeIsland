using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshAreaHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        OccupyListVO occupyList = JsonUtility.FromJson<OccupyListVO>(payload);

        RefreshUI.SetOccupyRefresh(occupyList);
    }
}
