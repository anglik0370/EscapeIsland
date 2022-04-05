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

    public void ConnectSocket(string ip, string port)
    {
        webSocket = new WebSocket($"ws://{ip}:{port}");
        webSocket.Connect();

        webSocket.OnMessage += (s, e) =>
        {
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
