using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVoteTimeHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        VoteManager.SetVoteTime(JsonUtility.FromJson<VoteTimeVO>(payload).voteTime);
    }
}
