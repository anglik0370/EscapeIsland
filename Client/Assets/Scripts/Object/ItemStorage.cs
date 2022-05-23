using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ItemStorage : MonoBehaviour, IInteractionObject
{
    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => ingameHandlerSO;

    public Action LobbyCallback => () => { };
    public Action IngameCallback => () => StoragePanel.Instance.Open();

    public bool CanInteraction => true;

    [SerializeField]
    private float interactionRange;
    public float InteractionRange => interactionRange;

    [SerializeField]
    private Transform interactionTrm;

    private SpriteRenderer sr;

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
        return interactionTrm;
    }

    public Sprite GetSprite()
    {
        return sr.sprite;
    }

    public bool GetFlipX()
    {
        return sr.flipX;
    }
}
