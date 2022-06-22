using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, IInteractionObject
{
    private SpriteRenderer sr;

    [SerializeField]
    private Transform interactionTrm;

    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => ingameHandlerSO;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    public bool CanInteraction => true;

    [SerializeField]
    private Collider2D interactionCol;
    public Collider2D InteractionCol => interactionCol;

    public Action LobbyCallback => () => SendManager.Instance.GameStart();
    public Action IngameCallback => () =>
    {
        if(missionType == MissionType.None)
        {
            SpawnerManager.Instance.PickUpSpawnerItem(this);
        }
        else if(missionType == MissionType.Charge)
        {
            MissionPanel.Instance.OpenGetMission(missionType, GetComponent<ItemCharger>());
        }
        else
        {
            MissionPanel.Instance.OpenGetMission(missionType);

            //if(MissionPanel.Instance.NeedCoolTimeMission(missionType))
            //    SendManager.Instance.StartMission(id, MissionType);
        }
    };

    public int id;

    private float maxCoolTime = 60f;
    private float curCoolTime = 0f;

    public bool isInteractionAble = true;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        interactionCol = GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        if(!isInteractionAble)
        {
            curCoolTime -= Time.deltaTime;

            if(curCoolTime <= 0f)
            {
                isInteractionAble = true;
            }
        }
    }

    public void StartTimer()
    {
        curCoolTime = maxCoolTime;
        isInteractionAble = false;
    }

    public void SetMaxCoolTime(float coolTime)
    {
        maxCoolTime = coolTime;
    }

    public float GetFillCoolTime()
    {
        return curCoolTime / maxCoolTime;
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Sprite GetSprite()
    {
        return sr != null ? sr.sprite : null;
    }

    public bool GetFlipX()
    {
        return sr != null ? sr.flipX : true;
    }
}
