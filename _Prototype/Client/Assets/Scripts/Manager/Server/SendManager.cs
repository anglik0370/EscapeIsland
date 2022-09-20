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
    public NeedItemSO NeedItemSO => needItemSO;

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
    
    public void Send(string type, VO vo)
    {
        DataVO dataVO = new DataVO(type, vo?.ToJson());
        SocketClient.SendDataToSocket(dataVO.ToJson());
    }

    public void Send(string type)
    {
        DataVO dataVO = new DataVO(type, null);
        SocketClient.SendDataToSocket(dataVO.ToJson());
    }

    public void ExitRoomSend()
    {
        Send("EXIT_ROOM");
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

            itemAmountList.Add(new ItemAmountVO(amount.item.itemId, amount.amount));
        }

        vo.data = new NeedItemVO(itemAmountList);

        DataVO dataVO = new DataVO("GameStart", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
