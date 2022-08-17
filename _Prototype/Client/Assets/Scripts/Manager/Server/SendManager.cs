using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendManager : MonoBehaviour
{
    public static SendManager Instance { get; set; }

    private int socketId;
    public int SocketId { get { return socketId; } set { socketId = value; } }
    private int roomNum;

    private Player user;

    [SerializeField]
    private NeedItemSO needItemSO;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            user = p;
            socketId = p.socketId;
            roomNum = p.roomNum;
        });
    }

    public void Send(string type)
    {
        DataVO dataVO = new DataVO(type, null);
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void Login(string name)
    {
        LoginVO vo = new LoginVO();
        vo.name = name;

        DataVO dataVO = new DataVO("LOGIN", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
    public void CreateRoom(string name, int curUserNum, int userNum, int kidnapperNum, bool isTest)
    {
        NetworkManager.instance.FindSetDataScript<RefreshUsers>().isTest = isTest;

        RoomVO vo = new RoomVO(name, 0, curUserNum, userNum, kidnapperNum);

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

        DataVO dataVO = new DataVO("EXIT_ROOM", null);

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void GameStart()
    {
        if (!user.master) return;

        RoomVO vo = new RoomVO();
        List<ItemAmountVO> itemAmountList = new List<ItemAmountVO>();
        vo.roomNum = roomNum;

        for (int i = 0; i < needItemSO.itemAmountList.Count; i++)
        {
            ItemAmount amount = needItemSO.itemAmountList[i];

            itemAmountList.Add(new ItemAmountVO(amount.item.itemId,amount.amount));
        }

        vo.data = new NeedItemVO(itemAmountList);

        DataVO dataVO = new DataVO("GameStart", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void ReqRoomRefresh()
    {
        DataVO dataVO = new DataVO("ROOM_REFRESH_REQ", "");
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendChat(string msg,ChatType chatType)
    {
        ChatVO vo = new ChatVO(socketId, msg, chatType);

        DataVO dataVO = new DataVO("CHAT", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void StartMission(int spawnerId,int senderId, MissionType missionType, Team team)
    {
        ItemSpawnerVO vo = new ItemSpawnerVO();
        vo.spawnerId = spawnerId;
        vo.missionType = missionType;
        vo.team = team;
        vo.senderId = senderId;

        DataVO dataVO = new DataVO("MISSION", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void StorageDrop(Team team,int itemSOId)
    {
        ItemStorageVO vo = new ItemStorageVO();
        vo.itemSOId = itemSOId;
        vo.team = team;

        DataVO dataVO = new DataVO("STORAGE_DROP", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void StorageFull()
    {
        DataVO dataVO = new DataVO("STORAGE_FULL", "");

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendSyncObj(bool isImmediate, SyncObjDataVO data, ObjType objType, BehaviourType behaviourType)
    {
        SyncObjVO vo = new SyncObjVO(isImmediate,objType, behaviourType, data);

        DataVO dataVO = new DataVO("SYNC_OBJ", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendCharacterChange(int charId, int beforeId)
    {
        CharacterVO vo = new CharacterVO(user.CurTeam,charId, beforeId, socketId);

        DataVO dataVO = new DataVO("CHARACTER_CHANGE", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendFindRoom(string roomName)
    {
        RoomVO vo = new RoomVO();
        vo.name = roomName;

        DataVO dataVO = new DataVO("FIND_ROOM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendAreaState(Area area)
    {
        UserVO vo = new UserVO();
        vo.area = area;
        DataVO dataVO = new DataVO("INSIDE_REFRESH", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendSabotage(int starterId, string sabotageName,Team team)
    {
        SabotageVO vo = new SabotageVO(starterId, sabotageName, team);

        DataVO dataVO = new DataVO("SABOTAGE", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendTrap(int socketId,int trapId)
    {
        TrapVO vo = new TrapVO(socketId, trapId);
        DataVO dataVO = new DataVO("ENTER_TRAP", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void EnterFlyPaper(int socketId, int id,int userId) 
    {
        FlyPaperVO vo = new FlyPaperVO(socketId, id,userId);
        DataVO dataVO = new DataVO("ENTER_FLY_PAPER", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendCantUseRefinery(CantUseRefineryVO vo)
    {
        DataVO dataVO = new DataVO("CANT_USE_REFINERY", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendNotLerpMove(NotLerpMoveVO vo)
    {
        DataVO dataVO = new DataVO("NOT_LERP_MOVE", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendSpawnerOpen(Area area, MissionType type,Team team, int spawnerId, bool isOpen)
    {
        OpenPanelVO vo = new OpenPanelVO(area, type, team, spawnerId, isOpen);
        DataVO dataVO = new DataVO("SPAWNER_OPEN", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendSKill(SkillVO vo)
    {
        DataVO dataVO = new DataVO("SKILL", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendAltar(AltarVO vo)
    {
        DataVO dataVO = new DataVO("ALTAR", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
