using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        LoginVO vo = JsonUtility.FromJson<LoginVO>(payload);
        NetworkManager.SetLoginData(vo.name, vo.socketId);
    }
}
