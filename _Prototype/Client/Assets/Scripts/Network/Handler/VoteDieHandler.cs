using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteDieHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        int dieSocId = int.Parse(payload);
        VoteManager.SetVoteDead(dieSocId);
    }
}
