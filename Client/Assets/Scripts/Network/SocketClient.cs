using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    private string url = "localhost";
    public int port = 31012;

    public GameObject handlerParent;

    private WebSocket webSocket; //�� ���� �ν��Ͻ�

    public static SocketClient instance;

    private Dictionary<string, IMsgHandler> handlerDic;

    private Queue<DataVO> packetList = new Queue<DataVO>();

    private WaitForSeconds threeSec;
    private Coroutine reConnectingCoroutine;

    public static void SendDataToSocket(string json)
    {
        instance.SendData(json);
    }

    private void Awake()
    {
        instance = this;
        handlerDic = new Dictionary<string, IMsgHandler>();

        threeSec = new WaitForSeconds(3f);
    }

    private void Start()
    {
        IMsgHandler[] handlerList = handlerParent.GetComponents<IMsgHandler>();
        for (int i = 0; i < handlerList.Length; i++)
        {
            //print(GetTypeString(handlerList[i].GetType().ToString()));
            handlerDic.Add(GetTypeString(handlerList[i].GetType().ToString()), handlerList[i]);
        }

        //webSocket = new WebSocket($"{url}:{port}");
        //ConnectSocket("localhost", port.ToString());
        ConnectSocket(url, port.ToString());

        reConnectingCoroutine = StartCoroutine(TryReConnecting());
    }

    IEnumerator TryReConnecting()
    {
        if (webSocket == null) yield break;

        yield return new WaitForSeconds(5f); //ó�� 5�� �����ҽð� �ְ�

        yield return new WaitUntil(() => webSocket.ReadyState != WebSocketState.Open); //������ ���涧���� ������ݴϴ�.

        while (webSocket.ReadyState != WebSocketState.Open) //websocket�� state�� open�� �ɶ����� 3�� �ֱ�� ������ ��õ�
        {
            yield return threeSec;
            ConnectSocket(url, port.ToString());
        };
        //���� �߰��� ������ ���� ����Ͽ� �ۼ��� �صδ� �ڵ� 
        NetworkManager.instance.BackLogin();
        PopupManager.instance.CloseAndOpen("login");

    }

    public string GetTypeString(string s)
    {
        List<int> idx = new List<int>();
        s = s.Replace("Handler", "");

        for (int i = 1; i < s.Length; i++)
        {
            if (s[i].Equals(char.ToUpper(s[i])))
            {
                idx.Add(i);
            }
        }

        for (int i = 0; i < idx.Count; i++)
        {
            if (i >= 1)
            {
                s = s.Insert(idx[i] + i, " ");
                continue;
            }
            s = s.Insert(idx[i], " ");
        }

        string[] strs = s.Split(' ');
        string returnStr = "";

        for (int i = 0; i < strs.Length; i++)
        {
            returnStr += strs[i];
            if (i + 1 != strs.Length) returnStr += "_";
        }


        return returnStr.ToUpper();
    }

    public void ConnectSocket(string ip, string port)
    {
        webSocket = new WebSocket($"ws://{ip}:{port}");
        webSocket.Connect();

        webSocket.OnMessage += (s, e) =>
        {
            //ReceiveData((WebSocket)s, e);
            DataVO vo = JsonUtility.FromJson<DataVO>(e.Data);
            packetList.Enqueue(vo);
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
        if(packetList.Count > 0)
        {
            IMsgHandler handler = null;
            DataVO vo = packetList.Dequeue();
            if (handlerDic.TryGetValue(vo.type, out handler))
            {
                //print(vo.type);
                handler.HandleMsg(vo.payload);
            }
            else
            {
                Debug.LogError($"�������� ���� �������� ��û {vo.type}");
                Debug.LogError(vo.payload);
            }
        }
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
