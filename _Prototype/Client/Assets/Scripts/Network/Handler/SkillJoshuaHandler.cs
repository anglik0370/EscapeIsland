using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillJoshuaHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        Skill.SetJoshuaSkill();
    }
}
