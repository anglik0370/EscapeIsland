using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissRapHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        Skill.SetDissRap();
    }

}
