using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;

    public int socketId = -1;
    public string socketName;
    public int roomNum;

    public object lockObj = new object();


    private Dictionary<int, Player> playerList = new Dictionary<int, Player>();
    private List<ISetAble> setDataScriptList = new List<ISetAble>();
    private Queue<int> removeSocketQueue = new Queue<int>();

    private Queue<ChatVO> chatQueue = new Queue<ChatVO>();

    public Transform setDataScriptsParent;

    private List<UserVO> tempDataList;
    private List<UserVO> gameOverUserList;

    private TimeVO timeVO;
    private VoteCompleteVO voteCompleteVO;
    private CharacterVO characterVO;

    private bool isLogin = false;
    private bool needMasterRefresh = false;
    private bool needVoteRefresh = false;
    private bool needTimeRefresh = false;
    private bool needVoteComplete = false;
    private bool endVoteTime = false;
    private bool needVoteDeadRefresh = false;
    private bool needWinRefresh = false;
    private bool needStorageFullRefresh = false;
    private bool needTimerRefresh = false;
    private bool needCharacterChangeRefresh = false;

    private int tempId = -1;
    private int curTime = -1;
    private MeetingType meetingType = MeetingType.EMERGENCY;
    private GameOverCase gameOverCase = GameOverCase.CollectAllItem;
    private string msg = string.Empty;
    private bool isTextChange = false;

    public bool isVoteTime = false;

    private Player user = null;
    public Player User
    {
        get { return user; }
        set { user = value; }
    }



    public GameObject map;

    public VotePopup voteTab;

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
        PoolManager.CreatePool<Player>(playerPrefab, transform, 2);

        setDataScriptList = setDataScriptsParent.GetComponents<ISetAble>().ToList();

        EventManager.SubEnterRoom(p => user = p);

        EventManager.SubBackToRoom(() =>
        {
            InitPlayers();
            voteTab.VoteUIDisable();
        });

        StartCoroutine(Frame());
    }

    IEnumerator Frame()
    {
        yield return null;
        map.SetActive(false);
    
    }

    public T FindSetDataScript<T>()
    {
        return (T)setDataScriptList.Find(x => x.GetType() == typeof(T));
    }

    public static void SetCharChange(CharacterVO vo)
    {
        lock(instance.lockObj)
        {
            instance.needCharacterChangeRefresh = true;
            instance.characterVO = vo;
        }
    }
    public static void SetTimerData(int curTime)
    {
        lock(instance.lockObj)
        {
            instance.needTimerRefresh = true;
            instance.curTime = curTime;
        }
    }
    public static void SetWinUserData(List<UserVO> list,int gameOverCase)
    {
        lock (instance.lockObj)
        {
            instance.needWinRefresh = true;
            instance.gameOverUserList = list;
            instance.gameOverCase = (GameOverCase)gameOverCase;
        }
    }

    public static void SetStorageFullData(string msg)
    {
        lock(instance.lockObj)
        {
            instance.needStorageFullRefresh = true;
            instance.msg = msg;
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

    public static void SetVoteTime(List<UserVO> list,int type)
    {
        lock (instance.lockObj)
        {
            instance.tempDataList = list;
            instance.meetingType = (MeetingType)type;
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

        if(needStorageFullRefresh)
        {
            SetStorageFull();
            needStorageFullRefresh = false;
        }
        
        if(needWinRefresh)
        {
            SetWinTeam();
            needWinRefresh = false;
        }

        if(needTimerRefresh)
        {
            TimerText();
            needTimerRefresh = false;
        }

        if(needCharacterChangeRefresh)
        {
            SetCharacterChange();
            needCharacterChangeRefresh = false;
        }

        while(chatQueue.Count > 0)
        {
            ChatVO vo = chatQueue.Dequeue();
            print("ChatHandler");

            Player p = null;

            if(playerList.TryGetValue(vo.socketId, out p))
            {
                if((!p.isDie && !user.isDie) || user.isDie)
                {
                    voteTab.CreateChat(false, p.socketName, vo.msg, p.curSO.profileImg);
                }
            }
            else
            {
                if(user.socketId == vo.socketId)
                {
                    voteTab.CreateChat(true, user.socketName, vo.msg, user.curSO.profileImg);
                }
            }
        }

        while (removeSocketQueue.Count > 0)
        {
            int soc = removeSocketQueue.Dequeue();
            playerList[soc].SetDisable(true);
            playerList[soc].RemoveCharacter();
            playerList.Remove(soc);
        }
    }

    public Dictionary<int,Player> GetPlayerDic()
    {
        return playerList;
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
        if(user != null)
        {
            return user.isKidnapper;
        }

        return false;
    }

    public void BackLogin()
    {
        //socketId = -1;
        socketName = "";
        roomNum = 0;

        map.SetActive(false);

        EventManager.OccurExitRoom();
    }

    public void EnterLobby()
    {
        print("enter lobby");
        ExitRoomSend();
    }

    public void SetStorageFull()
    {
        //msg띄워주기
    }

    //
    public void SetWinTeam()
    {
        //이긴 팀에 따라 해줘야 할 일 해주기
        print("GameOver실행");
        EventManager.OccurGameOver(gameOverCase);

        foreach (UserVO uv in gameOverUserList)
        {
            if (uv.socketId == socketId)
            {
                user.transform.position = uv.position;
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.transform.position = uv.position;
                }
            }
        }
    }
    public void InitPlayers()
    {
        user.InitPlayer();

        foreach (int idx in playerList.Keys)
        {
            playerList[idx].InitPlayer();
        }

        PlayerEnable(true);
    }

    public void TimerText()
    {
        if (isTextChange) return;
        voteTab.ChangeMiddleText(curTime.ToString());
    }

    IEnumerator TextChange(string msg)
    {
        isTextChange = true;
        voteTab.ChangeMiddleText(msg);

        yield return new WaitForSeconds(1f);

        isTextChange = false;
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
        isVoteTime = true;

        EventManager.OccurStartMeet(meetingType);
        StartCoroutine(TextChange("투표시간 시작"));

        foreach (UserVO uv in tempDataList)
        {
            if (uv.socketId == socketId)
            {
                user.transform.position = uv.position;
                voteTab.SetVoteUI(uv.socketId, uv.name, user.curSO.profileImg);
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.transform.position = uv.position;
                    voteTab.SetVoteUI(uv.socketId, uv.name, p.curSO.profileImg);
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

        //EndVoteTime();
    }
    

    public void SetCharacterChange()
    {
        CharacterProfile beforeProfile = CharacterSelectPanel.Instance.GetCharacterProfile(characterVO.beforeCharacterId);
        beforeProfile.BtnEnabled(true);

        CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(characterVO.characterId);
        profile.BtnEnabled(false);

        //characterVO.changerId -> 이 사람 캐릭터 바꿔주기
    }

    public void SetItemDisable(int spawnerId)
    {
        ItemSpawner s = SpawnerManager.Instance.SpawnerList.Find(x => x.id == spawnerId);
        s.DeSpawnItem();
    }

    public void SetItemStorage(int itemSOId)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(itemSOId);

        StorageManager.Instance.AddItem(so);
    }

    public void SetCharacter(CharacterSO so)
    {
        if (user == null) return;

        int beforeId = user.ChangeCharacter(so);

        CharacterVO vo = new CharacterVO(so.id, beforeId, socketId);

        DataVO dataVO = new DataVO("CHARACTER_CHANGE", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SetStartConvert(int converterId, int itemSOId)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(itemSOId);

        Debug.Log($"변환기{converterId}에서 {so}변환 시작");

        ConverterManager.Instance.ConverterList.Find(x => x.id == converterId).ConvertingStart(so);

        print("start");
    }

    public void SetResetConverter(int converterId)
    {
        ItemConverter converter = ConverterManager.Instance.ConverterList.Find(x => x.id == converterId);
        converter.ConvertingReset();
        print("reset");
    }

    public void SetTakeConverterAfterItem(int converterId)
    {
        ItemConverter converter = ConverterManager.Instance.ConverterList.Find(x => x.id == converterId);
        converter.TakeIAfterItem();
        //refinery.ingotItem = null;
        print("take");
    }

    public void EnterRoom()
    {
        //PopupManager.instance.CloseAndOpen("room");
        PopupManager.instance.ClosePopup();
        map.SetActive(true);
    }
    public void ExitRoom()
    {
        roomNum = 0;

        PlayerClear();
        map.SetActive(false);

        EventManager.OccurExitRoom();
        PopupManager.instance.CloseAndOpen("lobby");
    }

    public void GameEnd()
    {
        //EventManager.OccurExitRoom();
        EventManager.OccurBackToRoom();
        print("BackToRoom 실행");
    }

    public void PlayerClear()
    {
        user.StopCo();
        user.SetDisable(true);
        user = null;

        foreach (int key in playerList.Keys)
        {
            playerList[key].SetDisable();
        }
        playerList.Clear();
    }
    
    public void PlayerEnable(bool isEnd = false)
    {
        if (!user.isDie && !isEnd) return;

        foreach (int key in playerList.Keys)
        {
            if(!playerList[key].gameObject.activeSelf)
            {
                playerList[key].SetEnable();
            }
        }
    }
    
    
    public void RefreshMaster()
    {
        foreach (UserVO uv in tempDataList)
        {
            if (uv.socketId == socketId)
            {
                user.master = uv.master;
                //user.isImposter = uv.isImposter;
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if(p != null)
                {
                    p.master = uv.master;
                    //p.isImposter = uv.isImposter;
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
        isVoteTime = false;

        TimeHandler.Instance.InitKillCool();
        
        PopupManager.instance.ClosePopup();
        voteTab.VoteUIDisable();
    }

    public void Login(string name)
    {
        LoginVO vo = new LoginVO();
        vo.name = name;

        DataVO dataVO = new DataVO("LOGIN",JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
    public void CreateRoom(string name, int curUserNum, int userNum,int kidnapperNum,bool isTest)
    {
        FindSetDataScript<RefreshUsers>().isTest = isTest;

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

        //roomNum = 0;
        //EventManager.OccurExitRoom();
        //PlayerClear();

        DataVO dataVO = new DataVO("EXIT_ROOM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void GameStart()
    {
        //PopupManager.instance.CloseAndOpen("ingame");

        if (!user.master) return;

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

    public void StorageFull()
    {
        DataVO dataVO = new DataVO("STORAGE_FULL", "");

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void StartConverting(int refineryId, int itemSOId)
    {
        RefineryVO vo = new RefineryVO(refineryId, itemSOId);

        DataVO dataVO = new DataVO("START_CONVERTER", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void ResetConverter(int refineryId)
    {
        RefineryVO vo = new RefineryVO(refineryId, 0);

        DataVO dataVO = new DataVO("RESET_CONVERTER", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void TakeConverterAfterItem(int refineryId)
    {
        RefineryVO vo = new RefineryVO(refineryId, 0);

        DataVO dataVO = new DataVO("TAKE_CONVERTER", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

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

    public Player MakeRemotePlayer(UserVO data,CharacterSO so)
    {
        Player rpc = PoolManager.GetItem<Player>();
        InfoUI ui = InfoManager.SetInfoUI(rpc.transform, data.name);

        rpc.InitPlayer(data,ui, true,so);
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
