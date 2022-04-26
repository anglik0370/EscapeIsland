using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        SabotageVO vo = JsonUtility.FromJson<SabotageVO>(payload);

        Sabotage.SetSabotageData(vo);
    }
}
