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
    public Action IngameCallback => () => MeetManager.Instance.Meet(true);

    public bool CanInteraction => true;

    [SerializeField]
    private float interactionRange;
    public float InteractionRange => interactionRange;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
        return sr.sprite;
    }

    public bool GetFlipX()
    {
        return sr.flipX;
    }

    private InteractionSO GetInteractionSO()
    {
        if(PlayerManager.Instance.AmIDead())
        {
            return nothingSO;
        }

        return ingameHandlerSO;
    }
}
