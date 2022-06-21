using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTable : MonoBehaviour, IInteractionObject
{
    private SpriteRenderer sr;

    [SerializeField]
    private InteractionSO nothingSO;

    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => GetInteractionSO();

    public Action LobbyCallback => () => CharacterSelectPanel.Instance.Open();
    public Action IngameCallback => () => { };

    public bool CanInteraction => true;

    [SerializeField]
    private Collider2D interactionCol;
    public Collider2D InteractionCol => interactionCol;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        interactionCol = GetComponentInChildren<Collider2D>();
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            GameManager.Instance.AddInteractionObj(this);
        });
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Sprite GetSprite()
    {
        return sr.sprite;
    }

    public bool GetFlipX()
    {
        return sr.flipX;
    }

    private InteractionSO GetInteractionSO()
    {
        return ingameHandlerSO;
    }
}
