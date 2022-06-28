using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCherryHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        CherryVO vo = JsonUtility.FromJson<CherryVO>(payload);
    }
}
