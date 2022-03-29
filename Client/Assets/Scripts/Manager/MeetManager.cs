using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetManager : MonoBehaviour
{
    public static MeetManager Instance;

    private Player player;

    private LogTable meetingTable;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        meetingTable = FindObjectOfType<LogTable>();
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });
    }

    public void Meet(bool isEmergency = false)
    {
        if(isEmergency)
        {
            print("��� ȸ�� ����");

            SendManager.Instance.Send("EMERGENCY");
        }
        else
        {
            print("��ü �߰�");

            SendManager.Instance.Send("DEAD_REPORT");
        }
    }
}
