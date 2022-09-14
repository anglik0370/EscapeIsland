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

    private bool isOpen = false;
    public bool IsOpen => isOpen;

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
            MissionPanel.Instance.OpenGetMission(missionType, GetComponent<ItemCharger>(), this);
        }
        else
        {
            MissionPanel.Instance.OpenGetMission(missionType,null,this);

            //if(MissionPanel.Instance.NeedCoolTimeMission(missionType))
            //{
            //    isOpen = true;
            //}
            //    SendManager.Instance.StartMission(id, MissionType);
        }
    };

    public int id;

    private float maxCoolTime = 60f;
    private float curCoolTime = 0f;

    private float coolTimeMag = 1f;

    public bool isInteractionAble = true;

    public Area area;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        interactionCol = GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        if(!isInteractionAble)
        {
            curCoolTime -= (Time.deltaTime * coolTimeMag);

            if(curCoolTime <= 0f)
            {
                isInteractionAble = true;
            }
        }
    }

    public void StartTimer(float coolTimeMag = 0f)
    {
        curCoolTime = maxCoolTime;

        if (coolTimeMag != 0f)
        {
            this.coolTimeMag = coolTimeMag;
        }

        isInteractionAble = false;
    }

    public void SetMaxCoolTime(float coolTime)
    {
        maxCoolTime = coolTime;
    }

    public void SetOpen(bool isOpen)
    {
        if (NetworkManager.instance.User == null) return;

        this.isOpen = isOpen;

        SendManager.Instance.SendSpawnerOpen(area, missionType, NetworkManager.instance.User.CurTeam, id, isOpen,NetworkManager.instance.User.CanEnemyGathering);

        if(!isOpen)
        {
            SendManager.Instance.StartMission(id,NetworkManager.instance.socketId, MissionType,NetworkManager.instance.User.CurTeam);
        }
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
