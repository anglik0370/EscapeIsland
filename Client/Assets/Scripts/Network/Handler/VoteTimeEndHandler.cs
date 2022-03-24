using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteTimeEndHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        VoteManager.SetVoteEnd();
    }
}
