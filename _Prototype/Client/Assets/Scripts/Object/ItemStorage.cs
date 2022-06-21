using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ItemStorage : MonoBehaviour, IInteractionObject
{
    [SerializeField]
    private Team team;
    public Team Team => team;

    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => ingameHandlerSO;

    public Action LobbyCallback => () => SendManager.Instance.GameStart();
    public Action IngameCallback => () => MissionPanel.Instance.OpenStorageMission(team, item);

    public bool CanInteraction => true;

    [SerializeField]
    private Collider2D interactionCol;
    public Collider2D InteractionCol => interactionCol;

    private SpriteRenderer sr;

    [SerializeField]
    private ItemSO item;

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

    public void SetTeam(Team team)
    {
        this.team = team;
    }
}
