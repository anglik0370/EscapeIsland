using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //�� UI���ְ� �̵��� ��������ϴµ�
        NetworkManager.instance.EnterRoom(JsonUtility.FromJson<EnterRoomVO>(payload).selectedCharacterList);
    }
}
