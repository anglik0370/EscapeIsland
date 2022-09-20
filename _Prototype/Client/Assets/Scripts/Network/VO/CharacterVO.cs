using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterVO : VO
{
    public Team team;
    public int characterId;
    public int beforeCharacterId;
    public int changerId;

    public CharacterVO()
    {

    }

    public CharacterVO(Team team,int characterId, int beforeCharacterId, int changerId)
    {
        this.team = team;
        this.characterId = characterId;
        this.beforeCharacterId = beforeCharacterId;
        this.changerId = changerId;
    }
}
