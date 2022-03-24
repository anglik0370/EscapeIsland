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
        vo.roomNum = roomNum;

        DataVO dataVO = new DataVO("EXIT_ROOM", JsonUtility.ToJson(vo));

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

    public void SendCharacterChange(int charId, int beforeId)
    {
        CharacterVO vo = new CharacterVO(charId, beforeId, socketId);

        DataVO dataVO = new DataVO("CHARACTER_CHANGE", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
