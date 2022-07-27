using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshLobbyUiHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        LobbyUIVO vo = JsonUtility.FromJson<LobbyUIVO>(payload);

        //NetworkManager.SetUserCount(roomVO);
        RefreshUI.SetRefreshUI(vo);
    }
}
