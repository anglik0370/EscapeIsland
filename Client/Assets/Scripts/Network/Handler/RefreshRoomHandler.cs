using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshRoomHandler : IMsgHandler<RefreshRooms>
{

    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        RoomListVO roomList = JsonUtility.FromJson<RoomListVO>(payload);
        generic.SetRoomRefreshData(roomList.dataList);
    }
}
