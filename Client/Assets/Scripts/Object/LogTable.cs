using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTable : MonoBehaviour, IInteractionObject
{
    private SpriteRenderer sr;

    public Action<bool> Callback => isLobby =>
    {
        if (isLobby)
        {
            CharacterSelectPanel.Instance.Open();
        }
        else
        {
            MeetManager.Instance.Meet(true);
        }
    };

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

    public bool GetFlipX()
    {
        return sr.flipX;
    }
}
