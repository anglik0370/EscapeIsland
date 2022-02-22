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
        }
        else
        {
            print("��ü �߰�");
        }
    }
}
