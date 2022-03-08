using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRefreshHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //print(payload);
        TimeVO vo = JsonUtility.FromJson<TimeVO>(payload);
        NetworkManager.SetTimeRefresh(vo);
    }
}
