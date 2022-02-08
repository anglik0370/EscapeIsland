using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        ChatVO vo = JsonUtility.FromJson<ChatVO>(payload);

        NetworkManager.ReceiveChat(vo);
    }
}
