using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUserCountHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        RoomVO roomVO = JsonUtility.FromJson<RoomVO>(payload);

        NetworkManager.SetUserCount(roomVO);
    }
}
