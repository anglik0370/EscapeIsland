using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    public string url = "ws://localhost";
    public int port = 31012;

    public GameObject handlerParent;

    private WebSocket webSocket; //웹 소켓 인스턴스

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

    private void Start()
    {
        //핸들러 딕셔너리에 필요한것들 추가해주기
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
        handlerDic.Add("START_REFINERY", handlerParent.GetComponent<StartRefineryHandler>());
        handlerDic.Add("RESET_REFINERY", handlerParent.GetComponent<ResetRefineryHandler>());
        handlerDic.Add("TAKE_REFINERY", handlerParent.GetComponent<TakeRefineryHandler>());
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

        //webSocket = new WebSocket($"{url}:{port}");
        ConnectSocket("localhost", port.ToString());
        //25.17.255.82

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
                print(vo.type);
                handler.HandleMsg(vo.payload);
            }
            else
            {
                Debug.LogError($"존재하지 않은 프로토콜 요청 {vo.type}");
                Debug.LogError(vo.payload);
            }
        }
    }

    private void OnDestroy()
    {
        if (webSocket.ReadyState == WebSocketState.Connecting || webSocket.ReadyState == WebSocketState.Open)
            webSocket.Close();
    }
}
