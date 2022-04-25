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
        CharacterProfile beforeProfile = CharacterSelectPanel.Instance.GetCharacterProfile(characterVO.beforeCharacterId);
        beforeProfile.BtnEnabled(true);

        CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(characterVO.characterId);
        profile.BtnEnabled(false);

        //characterVO.changerId -> �� ��� ĳ���� �ٲ��ֱ�
    }

    public void ChangeCharacter(CharacterSO so)
    {
        if (user == null) return;

        int beforeId = user.ChangeCharacter(so);

        SendManager.Instance.SendCharacterChange(so.id, beforeId);
    }
}
