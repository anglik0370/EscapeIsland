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
    private bool needDieRefresh = false;

    private Player user = null;

    public GameObject[] lights;

    public CinemachineVirtualCamera followCam;
    public JoyStick roomJoyStick;
    public JoyStick inGameJoyStick;

    public Button startBtn;
    public GameObject map;
    public CanvasGroup ingameCanvas;

    public InteractionBtn interactionBtn;

    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("�ټ��� NetworkManager�� ������");
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
        map.SetActive(false);

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

    public static void SetDieData(List<UserVO> list)
    {
        lock(instance.lockObj)
        {
            instance.tempDataList = list;
            instance.needDieRefresh = true;
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

        if(needDieRefresh)
        {

            needDieRefresh = false;
        }

        while (removeSocketQueue.Count > 0)
        {
            int soc = removeSocketQueue.Dequeue();
            playerList[soc].SetDisable();
            playerList.Remove(soc);
        }
    }

    public void SocketDisconnect()
    {
        socketId = -1;
        socketName = "";
        roomNum = 0;
        once = false;
        interactionBtn.gameStart = false;
        map.SetActive(false);
        SetIngameCanvas(false);
    }

    public void EnterLobby()
    {
        interactionBtn.gameStart = false;
        ExitRoomSend();
    }

    public void SetIngameCanvas(bool enable)
    {
        ingameCanvas.alpha = enable ? 1f : 0f;
        ingameCanvas.interactable = enable;
        ingameCanvas.blocksRaycasts = enable;
    }

    public void OnGameStart()
    {
        PopupManager.instance.ClosePopup();
        SetIngameCanvas(true);
        interactionBtn.gameStart = true;



        foreach (UserVO uv in tempDataList)
        {
            if(uv.socketId == socketId)
            {
                inGameJoyStick.enabled = true;
             
                user.inventory = FindObjectOfType<Inventory>();
                interactionBtn.Init(user);
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

    public void SetItemDisable(int spawnerId)
    {
        ItemSpawner s = GameManager.Instance.spawnerList.Find(x => x.id == spawnerId);
        s.DeSpawnItem();
    }

    public void SetItemStorage(int itemSOId)
    {
        ItemSO so = GameManager.Instance.FindItemFromItemId(itemSOId);

        GameManager.Instance.AddItemInStorage(so);
    }

    public void SetStartRefinery(int refineryId, int itemSOId)
    {
        ItemSO so = GameManager.Instance.FindItemFromItemId(itemSOId);

        Debug.Log($"재련소{refineryId}에서 {so}재련 시작");

        GameManager.Instance.refineryList.Find(x => x.id == refineryId).StartRefining(so);

        print("start");
    }

    public void SetResetRefinery(int refineryId)
    {
        Refinery refinery = GameManager.Instance.refineryList.Find(x => x.id == refineryId);
        refinery.ResetRefining();
        print("reset");
    }

    public void SetTakeRefineryIngotItem(int refineryId)
    {
        Refinery refinery = GameManager.Instance.refineryList.Find(x => x.id == refineryId);
        refinery.TakeIngotItem();
        //refinery.ingotItem = null;
        print("take");
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
        SetIngameCanvas(false);
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
                //p�� ���ӿ�����Ʈ�� ���������� p�� �׾�����, ������ ���� �ʾ����� gameObject�� ���ش�

                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if(p != null)
                {
                    p.master = uv.master;
                    p.isImposter = uv.isImposter;
                }
            }
        }
    }
    public void RefreshDie()
    {
        foreach (UserVO uv in tempDataList)
        {
            if(uv.socketId != socketId)
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
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
                    //�ȷο� ķ ����
                    followCam.Follow = user.gameObject.transform;


                    once = true;
                }
            }
            
        }
    }

    public void RefreshTime(int day,bool isLightTime)
    {
        TimeHandler.Instance.TimeRefresh(day, isLightTime);
    }

    public void Login(string name)
    {
        LoginVO vo = new LoginVO();
        vo.name = name;

        DataVO dataVO = new DataVO("LOGIN",JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
    public void CreateRoom(string name, int curUserNum, int userNum,int kidnapperNum)
    {
        RoomVO vo = new RoomVO(name, 0,curUserNum, userNum,kidnapperNum);

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

    public void GetItem(int spawnerId)
    {
        ItemSpawnerVO vo = new ItemSpawnerVO();
        vo.spawnerId = spawnerId;

        DataVO dataVO = new DataVO("GET_ITEM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void StorageDrop(int itemSOId)
    {
        ItemStorageVO vo = new ItemStorageVO();
        vo.itemSOId = itemSOId;

        DataVO dataVO = new DataVO("STORAGE_DROP", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void StartRefinery(int refineryId, int itemSOId)
    {
        RefineryVO vo = new RefineryVO(refineryId, itemSOId);

        DataVO dataVO = new DataVO("START_REFINERY", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void ResetRefinery(int refineryId)
    {
        RefineryVO vo = new RefineryVO(refineryId, 0);

        DataVO dataVO = new DataVO("RESET_REFINERY", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void TakeRefineryIngotItem(int refineryId)
    {
        RefineryVO vo = new RefineryVO(refineryId, 0);

        DataVO dataVO = new DataVO("END_REFINERY", JsonUtility.ToJson(vo));

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
