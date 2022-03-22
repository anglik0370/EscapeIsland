using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private Queue<DataVO> packetList = new Queue<DataVO>();

    public static void SendDataToSocket(string json)
    {
        instance.SendData(json);
    }

    private void Awake()
    {
        instance = this;
        handlerDic = new Dictionary<string, IMsgHandler>();
    }

    public string GetTypeString(string s)
    {
        List<int> idx = new List<int>();
        s = s.Replace("Handler", "");

        for (int i = 1; i < s.Length; i++)
        {
            if(s[i].Equals(char.ToUpper(s[i])))
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

    private void Start()
    {
        //�ڵ鷯 ��ųʸ��� �ʿ��Ѱ͵� �߰����ֱ�
        string handlerPath = Application.dataPath + "/Scripts/Network/Handler/";
        //IMsgHandler[] test = Resources.LoadAll<>("/Handler");
        IMsgHandler[] handlerList = handlerParent.GetComponents<IMsgHandler>();
        for (int i = 0; i < handlerList.Length; i++)
        {
            print(GetTypeString(handlerList[i].GetType().ToString()));
        }

        

        //DirectoryInfo di = new DirectoryInfo(handlerPath);

        //FileInfo[] fileInfo = di.GetFiles();

        //for (int i = 0; i < fileInfo.Length; i++)
        //{
        //    print(fileInfo[i].);
        //}
        

        handlerDic.Add("LOGIN", handlerParent.GetComponent<LoginHandler>());
        handlerDic.Add("ENTER_ROOM", handlerParent.GetComponent<EnterRoomHandler>());
        handlerDic.Add("REFRESH_ROOM", handlerParent.GetComponent<RefreshRoomHandler>());
        handlerDic.Add("REFRESH_USER", handlerParent.GetComponent<RefreshUserHandler>());
        handlerDic.Add("REFRESH_MASTER", handlerParent.GetComponent<RefreshMasterHandler>());
        handlerDic.Add("GAME_START", handlerParent.GetComponent<GameStartHandler>());
        handlerDic.Add("EXIT_ROOM", handlerParent.GetComponent<ExitRoomHandler>());
        handlerDic.Add("ERROR", handlerParent.GetComponent<ErrorHandler>());
        handlerDic.Add("DISCONNECT", handlerParent.GetComponent<DisconnectHandler>());
        handlerDic.Add("GET_ITEM", handlerParent.GetComponent<GetItemHandler>());
        handlerDic.Add("STORAGE_DROP", handlerParent.GetComponent<StorageDropHandler>());
        handlerDic.Add("START_REFINERY", handlerParent.GetComponent<StartConvertingHandler>());
        handlerDic.Add("RESET_REFINERY", handlerParent.GetComponent<ResetConverterHandler>());
        handlerDic.Add("TAKE_REFINERY", handlerParent.GetComponent<TakeConverterHandler>());
        handlerDic.Add("TIME_REFRESH", handlerParent.GetComponent<TimeRefreshHandler>());
        handlerDic.Add("KILL", handlerParent.GetComponent<KillHandler>());
        handlerDic.Add("VOTE_TIME", handlerParent.GetComponent<VoteTimeHandler>());
        handlerDic.Add("CHAT", handlerParent.GetComponent<ChatHandler>());
        handlerDic.Add("VOTE_COMPLETE", handlerParent.GetComponent<VoteCompleteHandler>());
        handlerDic.Add("VOTE_DIE", handlerParent.GetComponent<VoteDieHandler>());
        handlerDic.Add("VOTE_TIME_END", handlerParent.GetComponent<VoteTimeEndHandler>());
        handlerDic.Add("STORAGE_FULL", handlerParent.GetComponent<StorageFullHandler>());
        handlerDic.Add("WIN_KIDNAPPER", handlerParent.GetComponent<WinKidnapperHandler>());
        handlerDic.Add("WIN_CITIZEN", handlerParent.GetComponent<WinCitizenHandler>());
        handlerDic.Add("TIMER", handlerParent.GetComponent<TimerHandler>());

        //webSocket = new WebSocket($"{url}:{port}");
        ConnectSocket("localhost", port.ToString());
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
        if (webSocket.ReadyState == WebSocketState.Connecting || webSocket.ReadyState == WebSocketState.Open)
            webSocket.Close();
    }

    private void OnApplicationQuit()
    {
        if (webSocket.ReadyState == WebSocketState.Connecting || webSocket.ReadyState == WebSocketState.Open)
            webSocket.Close();
    }
}
