using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetManager : MonoBehaviour
{
    public static MeetManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void Meet(bool isEmergency = false)
    {
        if(isEmergency)
        {
            print("긴급 회의 시작");

            DataVO dataVO = new DataVO("EMERGENCY", "");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }
        else
        {
            print("시체 발견");

            DataVO dataVO = new DataVO("DEAD_REPORT", "");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }
    }
}
