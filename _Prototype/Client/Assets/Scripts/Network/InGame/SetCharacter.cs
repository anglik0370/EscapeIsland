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

        if (playerList.ContainsKey(characterVO.changerId))
        {
            playerList[characterVO.changerId].ChangeCharacter(profile.GetSO());
        }
    }

    public void ChangeCharacter(CharacterSO so)
    {
        if (user == null) return;

        if (user.isReady)
        {
            UIManager.Instance.AlertText("준비 중엔 바꿀 수 없습니다.", AlertType.Warning);
            return;
        }

        int beforeId = user.ChangeCharacter(so);

        SendManager.Instance.Send("CHARACTER_CHANGE", new CharacterVO(user.CurTeam, so.id, beforeId, user.socketId));
    }
}
