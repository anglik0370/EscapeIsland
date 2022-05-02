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
        vo.roomNum = roomNum;

        DataVO dataVO = new DataVO("GameStart", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void ReqRoomRefresh()
    {
        DataVO dataVO = new DataVO("ROOM_REFRESH_REQ", "");
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendKill(int targetSocketId)
    {
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

    public void SendSyncObj(SyncObjDataVO data, ObjType objType, BehaviourType behaviourType)
    {
        SyncObjVO vo = new SyncObjVO(objType, behaviourType, data);

        DataVO dataVO = new DataVO("SYNC_OBJ", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendCharacterChange(int charId, int beforeId)
    {
        CharacterVO vo = new CharacterVO(charId, beforeId, socketId);

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

    public void SendInSide(bool isInside)
    {
        UserVO vo = new UserVO();
        vo.isInside = isInside;
        DataVO dataVO = new DataVO("INSIDE_REFRESH", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendSabotage(int starterId, bool isShareCoolTime, string sabotageName)
    {
        SabotageVO vo = new SabotageVO(starterId,isShareCoolTime, sabotageName);

        DataVO dataVO = new DataVO("SABOTAGE", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SendTrap(int trapId)
    {
        ObjVO vo = new ObjVO(trapId);
        DataVO dataVO = new DataVO("ENTER_TRAP", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
