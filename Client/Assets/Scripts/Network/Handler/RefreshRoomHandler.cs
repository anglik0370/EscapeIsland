using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshRoomHandler : MonoBehaviour, IMsgHandler
{
    private bool once = false;
    private RefreshRooms rooms;

    public void HandleMsg(string payload)
    {
        if(!once)
        {
            rooms = NetworkManager.instance.FindSetDataScript<RefreshRooms>();
            once = true;
        }
        RoomListVO roomList = JsonUtility.FromJson<RoomListVO>(payload);
        rooms.SetRoomRefreshData(roomList.dataList);
    }
}
