using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        ObjVO vo = JsonUtility.FromJson<ObjVO>(payload);

        Sabotage.SetExtinguishData(vo);
    }
}
