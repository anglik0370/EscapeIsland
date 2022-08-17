using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterFlyPaperHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        Skill.EnterFlyPaper(JsonUtility.FromJson<FlyPaperVO>(payload));
    }
}
