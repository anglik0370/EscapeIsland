using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        UserVO vo = JsonUtility.FromJson<UserVO>(payload);

        GameStart.SetGameReady(vo);
    }
}
