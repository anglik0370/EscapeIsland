using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemStorage : MonoBehaviour, IMapObject
{
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
}
