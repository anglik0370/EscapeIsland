using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour, IInteractionObject
{
    private SpriteRenderer sr;

    public Action<bool> Callback => isLobby =>
    {
        DeadBodyManager.Instance.ReportProximateDeadbody();
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

    public void Report()
    {
        MeetManager.Instance.Meet(false);

        DeadBodyManager.Instance.ClearDeadBody();
    }
}
