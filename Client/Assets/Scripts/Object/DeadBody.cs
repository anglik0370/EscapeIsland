using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour, IInteractionObject
{
    private const string ANIMT_DIE = "die";

    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => ingameHandlerSO;

    public Action LobbyCallback => () => { };
    public Action IngameCallback => () => Report();

    public bool CanInteraction => gameObject.activeSelf;

    public void Init(Vector3 pos, CharacterSO characterSO)
    {
        GameObject chara = CharacterSelectPanel.Instance.GetCharacterObj(characterSO.id);

        chara.transform.SetParent(transform);
        chara.transform.position = pos;

        Animator anim = chara.GetComponent<CharComponentHolder>().anim;

        anim.SetTrigger(ANIMT_DIE);
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Transform GetInteractionTrm()
    {
        return transform;
    }

    public Sprite GetSprite()
    {
        return null;
    }

    public bool GetFlipX()
    {
        return false;
    }

    public void Report()
    {
        MeetManager.Instance.Meet(false);

        DeadBodyManager.Instance.ClearDeadBody();
    }
}
