using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMissionHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //MissionPanel.Instance.OpenMissionPanel();
        NetworkManager.SetMissionPanel();
    }

}
