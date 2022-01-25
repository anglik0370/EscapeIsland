using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoomHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        NetworkManager.instance.ExitRoom();
    }
}
