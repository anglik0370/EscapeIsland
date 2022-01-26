using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
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
    private Queue<int> removeSocketQueue = new Queue<int>();
    private List<UserVO> userDataList;
    private List<UserVO> tempDataList;

    private bool isLogin = false;
    private bool once = false;
    private bool needRoomRefresh = false;
    private bool needUserRefresh = false;
    private bool needMasterRefresh = false;
    private bool needStartGame = false;

    private Player user = null;

    public GameObject[] lights;

    public CinemachineVirtualCamera followCam;
    public JoyStick roomJoyStick;
    public JoyStick inGameJoyStick;

    public Button startBtn;
    public GameObject map;
    public GameObject ingameCanvas;

    public InteractionBtn interactionBtn;

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
    public static void SetMasterRefreshData(List<UserVO> list)
    {
        lock (instance.lockObj)
        {
            instance.tempDataList = list;
            instance.needMasterRefresh = true;
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

    public static void GameStart(List<UserVO> list)
    {
        lock(instance.lockObj)
        {
            instance.tempDataList = list;
            instance.needStartGame = true;
        }
    }

    public static void DisconnectUser(int id)
    {
        lock(instance.lockObj)
        {
            instance.removeSocketQueue.Enqueue(id);
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

        if(needMasterRefresh)
        {
            RefreshMaster();

            needMasterRefresh = false;
        }

        if(needStartGame)
        {
            OnGameStart();

            needStartGame = false;
        }

        while (removeSocketQueue.Count > 0)
        {
            int soc = removeSocketQueue.Dequeue();
            playerList[soc].SetDisable();
            playerList.Remove(soc);
        }
    }

    public void InitData()
    {
        socketId = -1;
        socketName = "";
        roomNum = 0;
        once = false;
        map.SetActive(false);
        ingameCanvas.SetActive(false);
        TimeHandler.Instance.SetGame(false);
    }

    public void OnGameStart()
    {
        PopupManager.instance.ClosePopup();
        ingameCanvas.SetActive(true);
        TimeHandler.Instance.SetGame(true);

        foreach (UserVO uv in tempDataList)
        {
            if(uv.socketId == socketId)
            {
                inGameJoyStick.enabled = true;
                
                user.transform.position = uv.position;
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if(p != null)
                {
                    p.SetTransform(uv.position);
                }
            }
        }
    }

    public void EnterRoom()
    {
        PopupManager.instance.CloseAndOpen("room");
        map.SetActive(true);
    }
    public void ExitRoom()
    {
        PopupManager.instance.CloseAndOpen("lobby");
        map.SetActive(false);
    }

    public void PlayerClear()
    {
        user.StopCo();
        user.SetDisable();
        user = null;

        foreach (int key in playerList.Keys)
        {
            playerList[key].SetDisable();
        }
        playerList.Clear();
    }
    
    public void PlayerEnable()
    {
        if (!user.isDie) return;

        foreach (int key in playerList.Keys)
        {
            if(playerList[key].isDie && !playerList[key].gameObject.activeSelf)
            {
                playerList[key].gameObject.SetActive(true);
            }
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

            if(room == null)
            {
                room = Instantiate(roomEnterBtnPrefab, roomParent).GetComponent<RoomEnterBtn>();
                roomEnterBtnList.Add(room);
            }

            room.SetInfo(roomVO.name, roomVO.curUserNum, roomVO.userNum, roomVO.roomNum);
            room.gameObject.SetActive(true);
        }
    }
    public void RefreshMaster()
    {
        foreach (UserVO uv in tempDataList)
        {
            if (uv.socketId == socketId)
            {
                user.master = uv.master;
                startBtn.enabled = uv.master;
                user.isImposter = uv.isImposter;
            }
            else
            {
                //p의 게임오브젝트가 켜져있으며 p가 죽었을때, 유저가 죽지 않았을때 gameObject를 꺼준다

                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if(p != null)
                {
                    p.isDie = uv.isDie;
                    if (p.gameObject.activeSelf && p.isDie && !user.isDie)
                    {
                        p.SetDisable();
                    }
                }
            }
        }
        PlayerEnable();
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
                    user = PoolManager.GetItem<Player>();
                    user.InitPlayer(uv, false);

                    for (int i = 0; i < lights.Length; i++)
                    {
                        GameObject obj = Instantiate(lights[i], user.transform);
                        obj.transform.localPosition = Vector3.zero;
                    }

                    roomNum = uv.roomNum;
                    if(user.master)
                    {
                        startBtn.enabled = true;
                    }
                    roomJoyStick.player = user;
                    inGameJoyStick.player = user;
                    //팔로우 캠 설정
                    followCam.Follow = user.gameObject.transform;

                    interactionBtn.Init(user);

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
        RoomVO vo = new RoomVO();
        vo.roomNum = roomNum;

        DataVO dataVO = new DataVO("JOIN_ROOM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void ExitRoomSend()
    {
        RoomVO vo = new RoomVO();
        vo.roomNum = roomNum;

        roomNum = 0;
        once = false;
        startBtn.enabled = false;
        PlayerClear();

        DataVO dataVO = new DataVO("EXIT_ROOM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void GameStartBtn()
    {
        //PopupManager.instance.CloseAndOpen("ingame");
        
        

        RoomVO vo = new RoomVO();
        vo.roomNum = roomNum;

        DataVO dataVO = new DataVO("GameStart", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }


    public void Die()
    {
        DataVO dataVO = new DataVO("DIE", "");

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public Player MakeRemotePlayer(UserVO data)
    {
        Player rpc = PoolManager.GetItem<Player>();
        rpc.InitPlayer(data, true);
        rpc.SetTransform(data.position);

        playerList.Add(data.socketId, rpc);
        return null;
    }

    public void ReqRoomRefresh()
    {
        DataVO dataVO = new DataVO("ROOM_REFRESH_REQ", "");
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
