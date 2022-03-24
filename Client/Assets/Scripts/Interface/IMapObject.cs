using System;
using UnityEngine;

public interface IInteractionObject
{
    public Action<bool> Callback { get; }

    public Transform GetTrm();
    public Transform GetInteractionTrm();

    public Sprite GetSprite();
    public bool GetFlipX();
}
