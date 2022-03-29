using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sabotage",menuName = "SO/SabotageSO")]
public class SabotageSO : ScriptableObject
{
    public bool canSharing;
    public float coolTime;
    public Sprite sabotageSprite;
}
