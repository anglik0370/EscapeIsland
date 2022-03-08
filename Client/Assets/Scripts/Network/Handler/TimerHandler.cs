using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        int.Parse(payload);
    }
}
