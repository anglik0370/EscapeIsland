using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New InteractionHandler", menuName = "SO/InteractionHandlerSO")]
public class InteractionSO : ScriptableObject
{
    public InteractionCase interactoinCase;

    public Sprite btnSprite;

    public Action Callback;
}
