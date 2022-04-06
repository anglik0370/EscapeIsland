using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WebSocketSharp;

[Serializable]
public class DataVO
{
    public string type;
    public string payload;

    public DataVO()
    {

    }

    public DataVO(string type, string payload)
    {
        this.type = type;
        this.payload = payload;
    }
}

public interface IMsgHandler
{
    public void HandleMsg(string payload);
}

public class SocketClient : MonoBehaviour
{
    private string url = "localhost";
    public int port = 31012;

    private WebSocket webSocket; //웹 소켓 인스턴스

    public static SocketClient instance;

    private WaitForSeconds threeSec;
    private Coroutine reConnectingCoroutine;

    private object lockObj = new object();
    private bool needLoginRefresh = false;
    private bool needSetRoomRefresh = false;
    private UserVO loginData;
    private int roomNum;

    public static void SendDataToSocket(string json)
    {
        instance.SendData(json);
    }

    private void Awake()
    {
        instance = this;
        threeSec = new WaitForSeconds(3f);
    }

    private void Start()
    {
        ConnectSocket(url, port.ToString());

    }

    public static void SetLoginData(UserVO vo)
    {
        lock(instance.lockObj)
        {
            instance.needLoginRefresh = true;
            instance.loginData = vo;
        }
    }

    public static void SetRoomNum(int roomNum)
    {
        lock(instance.lockObj)
        {
            instance.roomNum = roomNum;
            instance.needSetRoomRefresh = true;
        }
    }

    public void ConnectSocket(string ip, string port)
    {
        webSocket = new WebSocket($"ws://{ip}:{port}");
        webSocket.Connect();

        webSocket.OnMessage += (s, e) =>
        {
            DataVO dataVo = JsonUtility.FromJson<DataVO>(e.Data);

            if(dataVo.type.Equals("LOGIN"))
            {
                SetLoginData(JsonUtility.FromJson<UserVO>(dataVo.payload));
            }

            if(dataVo.type.Equals("ENTER_ROOM"))
            {
                SetRoomNum(JsonUtility.FromJson<RoomVO>(dataVo.payload).roomNum);
            }
            print(e.Data);
        };
    }

    public void InitWebSocket()
    {
        if (webSocket == null) return;

        if (reConnectingCoroutine != null) StopCoroutine(reConnectingCoroutine);

        if (webSocket.ReadyState == WebSocketState.Connecting || webSocket.ReadyState == WebSocketState.Open)
            webSocket.Close();
        webSocket = null;
    }

    private void SendData(string json)
    {
        webSocket.Send(json);
    }

    private void Update()
    {
        if(needLoginRefresh)
        {
            Login();
            needLoginRefresh = false;
        }

        if(needSetRoomRefresh)
        {
            DebugManager.Instance.ChangeRoom(roomNum);
            needSetRoomRefresh = false;
        }
    }

    private void Login()
    {
        DebugManager.Instance.InitData(loginData.socketId, loginData.roomNum, loginData.name);
    }

    private void OnDestroy()
    {
        InitWebSocket();
    }

    private void OnApplicationQuit()
    {
        InitWebSocket();
    }
}
