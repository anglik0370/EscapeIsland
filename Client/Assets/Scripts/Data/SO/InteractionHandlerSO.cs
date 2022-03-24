using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New InteractionHandler", menuName = "SO/InteractionHandlerSO")]
public class InteractionHandlerSO : ScriptableObject
{
    public InteractionCase interactoinCase;

    public Sprite btnSprite;
    public string btnText = string.Empty;

    public Action Callback;
}
