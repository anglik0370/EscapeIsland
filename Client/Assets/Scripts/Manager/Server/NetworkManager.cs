using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;

    public int socketId = -1;
    public string socketName;
    public int roomNum;

    public object lockObj = new object();

    private List<RoomVO> roomList;
    private List<RoomEnterBtn> roomEnterBtnList = new List<RoomEnterBtn>();
    public GameObject roomEnterBtnPrefab;
    public Transform roomParent;

    private Dictionary<int, Player> playerList = new Dictionary<int, Player>();
    private List<UserVO> userDataList;


    private bool isLogin = false;
    private bool once = false;
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
        PoolManager.CreatePool<Player>(playerPrefab, transform, 10);

        StartCoroutine(Frame());
    }

    IEnumerator Frame()
    {
        yield return null;

        roomParent = FindObjectOfType<Lobby>().roomParent;

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

    public static void SetUserRefreshData(List<UserVO> list)
    {
        lock(instance.lockObj)
        {
            instance.userDataList = list;
            instance.needUserRefresh = true;
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
            PopupManager.instance.CloseAndOpen("lobby");
            isLogin = false;
        }

        if(needRoomRefresh)
        {
            RefreshRoom();
            needRoomRefresh = false;
        }

        if(needUserRefresh)
        {
            RefreshUser();
            needUserRefresh = false;
        }
    }

    public void InitData()
    {
        socketId = -1;
        socketName = "";
        roomNum = 0;
    }

    public void EnterRoom()
    {
        PopupManager.instance.CloseAndOpen("room");
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
            room.gameObject.SetActive(true);
        }
    }

    public void RefreshUser()
    {
        foreach (UserVO uv in userDataList)
        {
            if (uv.socketId != socketId)
            {
                Player p = null;
                playerList.TryGetValue(uv.socketId, out p);

                if (p == null)
                {
                    MakeRemotePlayer(uv);
                }
                else
                {
                    p.SetTransform(uv.position);
                }
            }
            else
            {
                if(!once)
                {
                    Player p = PoolManager.GetItem<Player>();
                    p.InitPlayer(uv, false);
                    //팔로우 캠 설정
                    once = true;
                }
            }
        }
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

    public Player MakeRemotePlayer(UserVO data)
    {
        Player rpc = PoolManager.GetItem<Player>();
        rpc.InitPlayer(data, true);

        playerList.Add(data.socketId, rpc);
        return null;
    }

    public void ReqRoomRefresh()
    {
        DataVO dataVO = new DataVO("ROOM_REFRESH_REQ", "");
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
