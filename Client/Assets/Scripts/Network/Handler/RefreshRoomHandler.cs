using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshRoomHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        RoomListVO roomList = JsonUtility.FromJson<RoomListVO>(payload);
        NetworkManager.SetRoomRefreshData(roomList.dataList);
    }
}
