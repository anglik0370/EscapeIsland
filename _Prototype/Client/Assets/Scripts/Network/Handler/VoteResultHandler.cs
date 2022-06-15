using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteResultHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        VoteManager.SetVoteResult();
    }
}
