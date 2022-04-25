using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        SetTimeVO vo = JsonUtility.FromJson<SetTimeVO>(payload);
        Timer.SetTime(vo);
    }
}
