using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //�� UI���ְ� �̵��� ��������ϴµ�
        PopupManager.instance.CloseAndOpen("room");

    }
}
