using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatHandler : IMsgHandler<Chat>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        ChatVO data = JsonUtility.FromJson<ChatVO>(payload);

        generic.ReceiveChat(data);
    }
}
