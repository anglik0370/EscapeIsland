using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUserHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        List<UserVO> list = JsonUtility.FromJson<List<UserVO>>(payload);
        NetworkManager.SetUserRefreshData(list);
    }
}
