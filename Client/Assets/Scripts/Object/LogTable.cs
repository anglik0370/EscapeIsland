using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTable : MonoBehaviour, IMapObject
{
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
        return transform;
    }

    public Sprite GetSprite()
    {
        return sr.sprite;
    }
}
