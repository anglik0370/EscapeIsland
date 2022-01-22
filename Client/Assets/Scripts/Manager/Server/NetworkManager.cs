using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;


    public int socketId = -1;
    public string socketName;
    public int roomNum;

    public object lockObj = new object();

    private bool isLogin = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 NetworkManager가 실행중");
            return;
        }
        instance = this;
    }


    public static void SetLoginData(string name, int socketId)
    {
        lock(instance.lockObj)
        {
            Debug.Log("SetLoginData");
            instance.socketName = name;
            instance.socketId = socketId;
            instance.isLogin = true;
        }
    }

    private void Update()
    {
        if(isLogin)
        {
            PopupManager.instance.ClosePopup();
            PopupManager.instance.OpenPopup("lobby");
            isLogin = false;
        }
    }

    public void InitData()
    {
        socketId = -1;
        socketName = "";
        roomNum = 0;
    }

    public void Login(string name)
    {
        LoginVO vo = new LoginVO();
        vo.name = name;

        DataVO dataVO = new DataVO("LOGIN",JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
    public void CreateRoom(string name, int userNum)
    {
        RoomVO vo = new RoomVO(name, 0, userNum);

        DataVO dataVO = new DataVO("CREATE_ROOM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
