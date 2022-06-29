using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRaiHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        Skill.SetRaiSkill();
    }
}
