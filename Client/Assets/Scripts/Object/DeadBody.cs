using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private Collider2D interactionCol;
    public Collider2D InteractionCol => interactionCol;

    private readonly Vector3 FLIP_ROT = new Vector3(0, 180, 0);
    private readonly Vector3 DEFAULT_ROT = Vector3.zero;
    private readonly Vector3 CREATE_POS = new Vector3(0f, -0.45f, 0f);

    private const float DEFAULT_SCALE_Z = 1;
    private const float FLIP_SCALE_Z = -1;

    private CharacterSO curSO;

    public void Init(Vector3 pos, bool isFlip, CharacterSO characterSO)
    {
        transform.position = pos;

        curSO = characterSO;

        interactionCol = GetComponentInChildren<Collider2D>();

        GameObject chara = CharacterSelectPanel.Instance.GetCharacterObj(characterSO.id);

        chara.transform.SetParent(transform);

        chara.transform.localPosition = isFlip ? new Vector3(-characterSO.adjsutPos.x, characterSO.adjsutPos.y, characterSO.adjsutPos.z) : characterSO.adjsutPos;
        chara.transform.rotation = Quaternion.Euler(isFlip ? FLIP_ROT : DEFAULT_ROT);
        chara.transform.localScale = new Vector3(chara.transform.localScale.x, chara.transform.localScale.y, isFlip ? FLIP_SCALE_Z : DEFAULT_SCALE_Z);

        Animator anim = chara.GetComponent<CharComponentHolder>().anim;

        chara.SetActive(true);

        anim.SetTrigger(ANIMT_DIE);
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Sprite GetSprite()
    {
        return null;
    }

    public CharComponentHolder GetCCH()
    {
        return GetComponentInChildren<CharComponentHolder>();
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
