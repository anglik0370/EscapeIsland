using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;


    public int socketId = -1;
    public string socketName;
    public int roomNum;

    public object lockObj = new object();

    private List<RoomVO> roomList;
    private List<RoomEnterBtn> roomEnterBtnList = new List<RoomEnterBtn>();
    public GameObject roomEnterBtnPrefab;
    public Transform roomParent;

    private List<UserVO> userList;


    private bool isLogin = false;
    private bool needRoomRefresh = false;
    private bool needUserRefresh = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 NetworkManager가 실행중");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            RoomEnterBtn room = Instantiate(roomEnterBtnPrefab, roomParent).GetComponent<RoomEnterBtn>();
            room.gameObject.SetActive(false);
            roomEnterBtnList.Add(room);
        }
    }

    public static void SetRoomRefreshData(List<RoomVO> list)
    {
        lock(instance.lockObj)
        {
            instance.roomList = list;
            instance.needRoomRefresh = true;
        }
    }

    public static void SetUserRefreshData(List<DataVO> list)
    {
        lock(instance.lockObj)
        {
            //foreach (UserVO item in list)
            //{
            //    if(item.)
            //}
        }
    }


    public static void SetLoginData(string name, int socketId)
    {
        lock(instance.lockObj)
        {
            instance.socketName = name;
            instance.socketId = socketId;
            instance.isLogin = true;
        }
    }

    private void Update()
    {
        if(isLogin)
        {
            PopupManager.instance.ClosePopup();
            PopupManager.instance.OpenPopup("lobby");
            isLogin = false;
        }

        if(needRoomRefresh)
        {

            needRoomRefresh = false;
        }
    }

    public void InitData()
    {
        socketId = -1;
        socketName = "";
        roomNum = 0;
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

            if(room == null)
            {
                room = Instantiate(roomEnterBtnPrefab, roomParent).GetComponent<RoomEnterBtn>();
                roomEnterBtnList.Add(room);
            }

            room.SetInfo(roomVO.name, roomVO.curUserNum, roomVO.userNum, roomVO.roomNum);

        }
    }

    public void RefreshUser()
    {

    }

    public void Login(string name)
    {
        LoginVO vo = new LoginVO();
        vo.name = name;

        DataVO dataVO = new DataVO("LOGIN",JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
    public void CreateRoom(string name, int curUserNum, int userNum)
    {
        RoomVO vo = new RoomVO(name, 0,curUserNum, userNum);

        DataVO dataVO = new DataVO("CREATE_ROOM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void JoinRoom(int roomNum)
    {

    }
}
