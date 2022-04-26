using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrapHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        ObjVO vo = JsonUtility.FromJson<ObjVO>(payload);
        Sabotage.SetTrapData(vo.objId);
    }
}
