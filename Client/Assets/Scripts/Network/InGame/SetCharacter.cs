using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharacter : ISetAble
{
    private bool needCharacterChangeRefresh = false;

    private CharacterVO characterVO;

    void Update()
    {
        if (needCharacterChangeRefresh)
        {
            SetCharacterChange();
            needCharacterChangeRefresh = false;
        }
    }

    public void SetCharChange(CharacterVO vo)
    {
        lock (lockObj)
        {
            needCharacterChangeRefresh = true;
            characterVO = vo;
        }
    }

    public void SetCharacterChange()
    {
        Init();

        CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(characterVO.characterId);

        print($"{characterVO.changerId} change {characterVO.characterId}");
        if(playerList.ContainsKey(characterVO.changerId))
        {
            playerList[characterVO.changerId].ChangeCharacter(profile.GetSO());
            print("change");
        }
    }

    public void ChangeCharacter(CharacterSO so)
    {
        if (user == null) return;

        int beforeId = user.ChangeCharacter(so);

        SendManager.Instance.SendCharacterChange(so.id, beforeId);
    }
}
