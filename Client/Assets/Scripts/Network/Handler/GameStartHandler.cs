using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        NetworkManager.GameStart();
    }
}
