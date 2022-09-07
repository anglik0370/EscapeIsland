using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupyHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        Occupy.SetOccupyData(JsonUtility.FromJson<OccupyVO>(payload));
    }
}
