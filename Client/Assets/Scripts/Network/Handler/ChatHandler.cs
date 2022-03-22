using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        ChatVO data = JsonUtility.FromJson<ChatVO>(payload);

        NetworkManager.ReceiveChat(data);
    }
}
