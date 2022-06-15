using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        NetworkManager.DisconnectUser(int.Parse(payload));
    }

}
