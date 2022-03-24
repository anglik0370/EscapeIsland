using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ItemStorage : MonoBehaviour, IInteractionObject
{
    public Action<bool> Callback => isLobby =>
    {
        StoragePanel.Instance.Open();
    };

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
