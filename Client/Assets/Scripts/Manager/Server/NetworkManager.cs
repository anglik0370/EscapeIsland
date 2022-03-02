using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;

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

    private Queue<ChatVO> chatQueue = new Queue<ChatVO>();

    private List<UserVO> userDataList;
    private List<UserVO> tempDataList;

    private TimeVO timeVO;
    private VoteCompleteVO voteCompleteVO;

    private bool isLogin = false;
    private bool once = false;
    private bool needRoomRefresh = false;
    private bool needUserRefresh = false;
    private bool needMasterRefresh = false;
    private bool needStartGame = false;
    private bool needDieRefresh = false;
    private bool needVoteRefresh = false;
    private bool needTimeRefresh = false;
    private bool needVoteComplete = false;
    private bool endVoteTime = false;
    private bool needVoteDeadRefresh;

    private int tempId = -1;

    private Player user = null;

    public GameObject[] lights;

    public CinemachineVirtualCamera followCam;
    public JoyStick roomJoyStick;
    public JoyStick inGameJoyStick;

    public Button startBtn;
    public GameObject map;
    public CanvasGroup ingameCanvas;

    public InteractionBtn interactionBtn;

    public Vote voteTab;

    public List<AreaCover> covers = new List<AreaCover>();

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


    public static void SetTimeRefresh(TimeVO vo)
    {
        lock (instance.lockObj)
        {
            instance.timeVO = vo;
            instance.needTimeRefresh = true;
        }
    }

    public static void SetVoteTime(List<UserVO> list)
    {
        lock (instance.lockObj)
        {
            instance.tempDataList = list;
            instance.needVoteRefresh = true;
        }
    }

    public static void SetVoteDead(int deadId)
    {
        lock(instance.lockObj)
        {
            instance.needVoteDeadRefresh = true;
            instance.tempId = deadId;
        }
    }

    public static void SetVoteEnd()
    {
        lock(instance.lockObj)
        {
            instance.endVoteTime = true;
        }
    }

    public static void SetVoteComplete(VoteCompleteVO vo)
    {
        lock (instance.lockObj)
        {
            instance.voteCompleteVO = vo;
            instance.needVoteComplete = true;
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

    public static void ReceiveChat(ChatVO vo)
    {
        lock(instance.lockObj)
        {
            instance.chatQueue.Enqueue(vo);
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
        if(needTimeRefresh)
        {
            RefreshTime(timeVO.day, timeVO.isLightTime);
            needTimeRefresh = false;
        }

        if(needStartGame)
        {
            OnGameStart();

            needStartGame = false;
        }

        if(needDieRefresh)
        {
            RefreshDie();
            needDieRefresh = false;
        }

        if(needVoteRefresh)
        {
            OnVoteTimeStart();
            needVoteRefresh = false;
        }

        if(needVoteComplete)
        {
            VoteComplete();
            needVoteComplete = false;
        }

        if(needVoteDeadRefresh)
        {
            SetDeadRefresh();
            needVoteDeadRefresh = false;
        }

        if(endVoteTime)
        {
            EndVoteTime();
            endVoteTime = false;
        }

        while(chatQueue.Count > 0)
        {
            ChatVO vo = chatQueue.Dequeue();

            Player p = null;
            playerList.TryGetValue(vo.socketId, out p);

            if(p != null)
            {
                if((!p.isDie && !user.isDie) || user.isDie)
                {
                    voteTab.CreateChat(false, p.socketName, vo.msg, p.charSprite);
                }
                voteTab.chatRect.verticalNormalizedPosition = 0.0f;
            }
            else
            {
                if(user.socketId == vo.socketId)
                {
                    voteTab.CreateChat(true, user.socketName, vo.msg, user.charSprite);
                }
            }
        }

        while (removeSocketQueue.Count > 0)
        {
            int soc = removeSocketQueue.Dequeue();
            playerList[soc].SetDisable();
            playerList.Remove(soc);
        }
    }

    public List<Player> GetPlayerList()
    {
        return playerList.Values.ToList();
    }

    public bool GetPlayerDie()
    {
        return user.isDie;
    }

    public bool GetPlayerDie(int socId)
    {
        Player p = null;

        playerList.TryGetValue(socId, out p);

        

        return p == null ? false : p.isDie;
    }

    public bool IsKidnapper()
    {
        return user.isImposter;
    }

    public void SocketDisconnect()
    {
        socketId = -1;
        socketName = "";
        roomNum = 0;
        once = false;
        interactionBtn.isGameStart = false;
        map.SetActive(false);
        SetIngameCanvas(false);
    }

    public void EnterLobby()
    {
        print("enter lobby");
        ExitRoomSend();
    }

    public void SetIngameCanvas(bool enable)
    {
        ingameCanvas.alpha = enable ? 1f : 0f;
        ingameCanvas.interactable = enable;
        ingameCanvas.blocksRaycasts = enable;
    }

    public void StopOrPlay(bool on)
    {
        inGameJoyStick.SetEnable(on);
        interactionBtn.enabled = on;
    }

    public void VoteComplete()
    {
        VoteUI ui = voteTab.FindVoteUI(voteCompleteVO.voterId);
        ui.VoteComplete();

        if (voteCompleteVO.voterId == socketId)
        {
            voteTab.CompleteVote();
        }
        
    }

    public void OnVoteTimeStart()
    {
        StopOrPlay(false);
        PopupManager.instance.OpenPopup("vote");
        

        foreach (UserVO uv in tempDataList)
        {
            if (uv.socketId == socketId)
            {
                user.transform.position = uv.position;
                voteTab.SetVoteUI(uv.socketId, uv.name, user.charSprite);
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.transform.position = uv.position;
                    voteTab.SetVoteUI(uv.socketId, uv.name, p.charSprite);
                }
            }
            
        }
    }
    public void SetDeadRefresh()
    {
        if(tempId == socketId)
        {
            user.SetDead();

        }
        else if(playerList.ContainsKey(tempId))
        {
            Player p = playerList[tempId];

            p.SetDead();

            if (p.gameObject.activeSelf && p.isDie && !user.isDie)
            {
                p.SetDisable();
            }
        }
        PlayerEnable();

        EndVoteTime();
    }
    public void OnGameStart()
    {
        PopupManager.instance.ClosePopup();
        SetIngameCanvas(true);

        //interactionBtn.gameStart = true;

        foreach (UserVO uv in tempDataList)
        {
            if(uv.socketId == socketId)
            {
                inGameJoyStick.enabled = true;
             
                user.transform.position = uv.position;

                EventManager.OccurGameStart(user);
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if(p != null)
                {
                    //p.SetTransform(uv.position);
                    p.transform.position = uv.position;
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
        PlayerClear();
        map.SetActive(false);
        SetIngameCanvas(false);
        PopupManager.instance.CloseAndOpen("lobby");
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
                playerList[key].SetEnable();
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

            room.SetInfo(roomVO.name, roomVO.curUserNum, roomVO.userNum, roomVO.roomNum,roomVO.kidnapperNum);
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
            if(uv.socketId == socketId)
            {
               if(uv.isDie)
                {
                    user.SetDead();
                }
                
            }
            else //if(uv.socketId != socketId)
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    if (uv.isDie)
                    {
                        //p.isDie = uv.isDie;
                        p.SetDead();
                    }
                    
                    if (p.gameObject.activeSelf && uv.isDie && !user.isDie)
                    {
                        p.SetDisable();
                        p.SetDeadBody();
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
                    InfoUI ui = InfoManager.SetInfoUI(user.transform, uv.name);
                    user.InitPlayer(uv, ui, false);


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

                    EventManager.OccurEnterRoom(user);
                }
            }
            
        }
    }

    public void RefreshTime(int day,bool isLightTime)
    {
        EndVoteTime();
        TimeHandler.Instance.TimeRefresh(day, isLightTime);
    }
    
    public void EndVoteTime()
    {
        TimeHandler.Instance.endTime = 15f;
        GameManager.Instance.ClearDeadBody();
        PopupManager.instance.ClosePopup();
        voteTab.VoteUIDisable();
        StopOrPlay(true);
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
        interactionBtn.isGameStart = false;
        //PlayerClear();

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

        DataVO dataVO = new DataVO("TAKE_REFINERY", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }


    //public void Die()
    //{
    //    DataVO dataVO = new DataVO("DIE", "");

    //    SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    //}

    public void Kill(Player targetPlayer)
    {
        int targetSocketId = 0;

        foreach (int socketId in playerList.Keys)
        {
            if(playerList[socketId] == targetPlayer)
            {
                targetSocketId = socketId;
                break;
            }
        }

        KillVO vo = new KillVO(targetSocketId);

        DataVO dataVO = new DataVO("KILL", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendChat(string msg)
    {
        ChatVO vo = new ChatVO(socketId, msg);

        DataVO dataVO = new DataVO("CHAT", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public Player MakeRemotePlayer(UserVO data)
    {
        Player rpc = PoolManager.GetItem<Player>();
        InfoUI ui = InfoManager.SetInfoUI(rpc.transform, data.name);
        rpc.InitPlayer(data,ui, true);
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
