using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteTimeHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        UserListVO vo = JsonUtility.FromJson<UserListVO>(payload);

        NetworkManager.SetVoteTime(vo.dataList);
    }
}
