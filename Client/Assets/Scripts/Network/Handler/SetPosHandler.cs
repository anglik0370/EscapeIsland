using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        UserListVO vo = JsonUtility.FromJson<UserListVO>(payload);
        NetworkManager.SetPos(vo.dataList);
    }
}
