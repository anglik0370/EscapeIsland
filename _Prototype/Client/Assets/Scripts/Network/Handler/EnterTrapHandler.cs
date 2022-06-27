using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrapHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        TrapVO vo = JsonUtility.FromJson<TrapVO>(payload);
        Sabotage.SetTrapData(vo);
    }
}
