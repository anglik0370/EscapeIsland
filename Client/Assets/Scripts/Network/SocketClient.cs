using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    public string url = "ws://localhost";
    public int port = 31012;

    public GameObject handlerParent;

    private WebSocket webSocket; //�� ���� �ν��Ͻ�

    public static SocketClient instance;

    private Dictionary<string, IMsgHandler> handlerDic;

    public static void SendDataToSocket(string json)
    {
        instance.SendData(json);
    }

    private void Awake()
    {
        instance = this;
        handlerDic = new Dictionary<string, IMsgHandler>();
    }

    private void Start()
    {
        //�ڵ鷯 ��ųʸ��� �ʿ��Ѱ͵� �߰����ֱ�
        handlerDic.Add("LOGIN", handlerParent.GetComponent<LoginHandler>());
        handlerDic.Add("ENTER_ROOM", handlerParent.GetComponent<EnterRoomHandler>());
        handlerDic.Add("REFRESH_ROOM", handlerParent.GetComponent<RefreshRoomHandler>());
        handlerDic.Add("REFRESH_USER", handlerParent.GetComponent<RefreshUserHandler>());


        //webSocket = new WebSocket($"{url}:{port}");
        ConnectSocket("localhost", port.ToString());
        PopupManager.instance.OpenPopup("login");
    }

    public void ConnectSocket(string ip, string port)
    {
        webSocket = new WebSocket($"ws://{ip}:{port}");
        webSocket.Connect();

        webSocket.OnMessage += (s, e) =>
        {
            ReceiveData((WebSocket)s, e);
        };
    }
    public void InitWebSocket()
    {
        if (webSocket.ReadyState == WebSocketState.Connecting || webSocket.ReadyState == WebSocketState.Open)
            webSocket.Close();
        webSocket = null;
    }
    private void ReceiveData(WebSocket sender, MessageEventArgs e)
    {
        DataVO vo = JsonUtility.FromJson<DataVO>(e.Data);

        IMsgHandler handler = null;

        if(handlerDic.TryGetValue(vo.type, out handler))
        {
            handler.HandleMsg(vo.payload);
        }
        else
        {
            Debug.LogError($"�������� ���� �������� ��û {vo.type}");
            Debug.LogError(vo.payload);
        }
    }

    private void SendData(string json)
    {
        webSocket.Send(json);
    }

    private void OnDestroy()
    {
        if (webSocket.ReadyState == WebSocketState.Connecting || webSocket.ReadyState == WebSocketState.Open)
            webSocket.Close();
    }
}
