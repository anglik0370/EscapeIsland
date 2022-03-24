using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangeHandler : MonoBehaviour,IMsgHandler
{
    private bool once = false;
    private SetCharacter sc;
    public void HandleMsg(string payload)
    {
        if(!once)
        {
            sc = NetworkManager.instance.FindSetDataScript<SetCharacter>();
            once = true;
        }
        sc.SetCharChange(JsonUtility.FromJson<CharacterVO>(payload));
    }
}
