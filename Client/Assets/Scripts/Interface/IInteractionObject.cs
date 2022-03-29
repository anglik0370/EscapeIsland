using System;
using UnityEngine;

public interface IInteractionObject
{
    public InteractionSO LobbyHandlerSO { get; }
    public InteractionSO InGameHandlerSO { get; }

    public Action LobbyCallback { get; }
    public Action IngameCallback { get; }

    public bool CanInteraction { get; }

    public Transform GetTrm();
    public Transform GetInteractionTrm();

    public Sprite GetSprite();
    public bool GetFlipX();
}
