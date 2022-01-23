using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //룸 UI켜주고 이동을 시켜줘야하는데
        PopupManager.instance.CloseAndOpen("room");

    }
}
