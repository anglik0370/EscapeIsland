using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantUseRefineryHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        CantUseRefineryVO vo = JsonUtility.FromJson<CantUseRefineryVO>(payload);

        Sabotage.SetRefineryData(vo);
    }
}
