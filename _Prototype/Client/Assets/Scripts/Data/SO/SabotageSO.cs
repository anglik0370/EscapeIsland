using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Sabotage",menuName = "SO/SabotageSO")]
public class SabotageSO : ScriptableObject
{
    public string sabotageName;
    public Sprite sabotageSprite;

    public UnityEvent callback;
}
