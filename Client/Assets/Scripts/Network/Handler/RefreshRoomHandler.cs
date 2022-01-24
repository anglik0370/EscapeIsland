using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshRoomHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        List<RoomVO> roomList = JsonUtility.FromJson<List<RoomVO>>(payload);
        NetworkManager.SetRoomRefreshData(roomList);
    }
}
