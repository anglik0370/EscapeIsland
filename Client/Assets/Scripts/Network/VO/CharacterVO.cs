using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterVO
{
    public int characterId;
    public int changerId;

    public CharacterVO()
    {

    }

    public CharacterVO(int characterId, int changerId)
    {
        this.characterId = characterId;
        this.changerId = changerId;
    }
}
