using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangeHandler : IMsgHandler<SetCharacter>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        generic.SetCharChange(JsonUtility.FromJson<CharacterVO>(payload));
    }
}
