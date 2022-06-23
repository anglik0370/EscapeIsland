using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTeamHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        ChangeTeamVO changeTeamVO = JsonUtility.FromJson<ChangeTeamVO>(payload);
        NetworkManager.SetChangeTeam(changeTeamVO);
    }
}
