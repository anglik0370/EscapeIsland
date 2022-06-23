using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIanHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        Skill.SetRemoveAllDebuff();
    }
}
