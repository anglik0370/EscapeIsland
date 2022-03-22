using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangeHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        NetworkManager.SetCharChange(JsonUtility.FromJson<CharacterVO>(payload));
    }
}
