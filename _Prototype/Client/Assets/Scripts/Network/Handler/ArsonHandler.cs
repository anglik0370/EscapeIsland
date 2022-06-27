using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsonHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        ArsonVO vo = JsonUtility.FromJson<ArsonVO>(payload);

        Sabotage.SetArsonData(vo);
    }
}
