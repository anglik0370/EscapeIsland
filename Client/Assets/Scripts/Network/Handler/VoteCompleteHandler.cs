using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteCompleteHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        print(payload);
        VoteCompleteVO vo = JsonUtility.FromJson<VoteCompleteVO>(payload);
        NetworkManager.SetVoteComplete(vo);
    }

}
