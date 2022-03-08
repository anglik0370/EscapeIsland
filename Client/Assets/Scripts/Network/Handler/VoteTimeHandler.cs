using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteTimeHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MeetingVO vo = JsonUtility.FromJson<MeetingVO>(payload);

        NetworkManager.SetVoteTime(vo.dataList,vo.type);
    }
}
