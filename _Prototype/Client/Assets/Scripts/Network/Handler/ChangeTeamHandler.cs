using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTeamHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        UserVO uv = JsonUtility.FromJson<UserVO>(payload);
        NetworkManager.SetChangeTeam(uv);
    }
}
