using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshRooms : ISetAble
{
    private bool needRoomRefresh = false;

    private List<RoomVO> roomList;

    private List<RoomEnterBtn> roomEnterBtnList = new List<RoomEnterBtn>();

    public GameObject roomEnterBtnPrefab;

    private Transform roomParent;

    public void SetRoomRefreshData(List<RoomVO> list)
    {
        lock (lockObj)
        {
            roomList = list;
            needRoomRefresh = true;
        }
    }

    protected override void Start()
    {
        StartCoroutine(Frame());
    }

    IEnumerator Frame()
    {
        yield return null;

        roomParent = FindObjectOfType<LobbyPopup>().roomParent;

        for (int i = 0; i < 10; i++)
        {
            RoomEnterBtn room = Instantiate(roomEnterBtnPrefab, roomParent).GetComponent<RoomEnterBtn>();
            room.gameObject.SetActive(false);
            roomEnterBtnList.Add(room);
        }
    }

    private void Update()
    {
        if (needRoomRefresh)
        {
            RefreshRoom();
            needRoomRefresh = false;
        }
    }

    public void RefreshRoom()
    {
        for (int i = 0; i < roomEnterBtnList.Count; i++)
        {
            roomEnterBtnList[i].gameObject.SetActive(false);
        }

        foreach (RoomVO roomVO in roomList)
        {
            RoomEnterBtn room = roomEnterBtnList.Find(x => !x.gameObject.activeSelf);

            if (room == null)
            {
                room = Instantiate(roomEnterBtnPrefab, roomParent).GetComponent<RoomEnterBtn>();
                roomEnterBtnList.Add(room);
            }

            room.SetInfo(roomVO.name, roomVO.curUserNum, roomVO.userNum, roomVO.roomNum, roomVO.kidnapperNum);
            room.gameObject.SetActive(true);
        }
    }
}
