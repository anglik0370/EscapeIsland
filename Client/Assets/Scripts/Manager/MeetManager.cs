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
            print("��� ȸ�� ����");

            DataVO dataVO = new DataVO("EMERGENCY", "");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }
        else
        {
            print("��ü �߰�");

            DataVO dataVO = new DataVO("DEAD_REPORT", "");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }
    }
}
