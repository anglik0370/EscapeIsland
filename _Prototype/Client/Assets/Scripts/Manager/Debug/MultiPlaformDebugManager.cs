using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MultiPlaformDebugManager : MonoBehaviour
{
    private DebugVO vo = null;

    private bool once = false;

    private void Start()
    {
        if (!File.Exists($"{Application.dataPath}/Debug.json")) return;

        string json = File.ReadAllText($"{Application.dataPath}/Debug.json");
        vo = JsonUtility.FromJson<DebugVO>(json);
    }

    private void Update()
    {
        if (vo == null) return;

        if(!once)
        {
            if (vo.autoLogin) Login();

            if (vo.createRoom) CreateRoom();
            else JoinRoom();

            if (vo.gameStart) StartGame();

            if (vo.isKidnapper) AddKidnapperList();

            PopupManager.instance.ClosePopup();
            once = true;
        }
    }

    private void Login()
    {
        SendManager.Instance.Login($"User{vo.clientId}");
    }

    private void CreateRoom()
    {
        DataVO dataVO = new DataVO("REMOVE_ALL_ROOM", "");
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        SendManager.Instance.CreateRoom("Room", 0, 10, 1, false);
    }

    private void JoinRoom()
    {
        SendManager.Instance.JoinRoom(1);
    }

    private void StartGame()
    {
        SendManager.Instance.GameStart();
    }

    private void AddKidnapperList()
    {
        
    }
}

/*
 * 안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~
 * 안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~안녕안녕
 * 나는 돼지야
 * 배달음식 쳐먹어서 이렇게 됐지~
 * 
 */