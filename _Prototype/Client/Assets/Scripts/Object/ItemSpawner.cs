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
        if (MissionPanel.Instance.IsGetMissionPanelOpen) return;

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

    private void Start()
    {
        EventManager.SubExitRoom(() =>
        {
            curCoolTime = 0f;
            isInteractionAble = true;
        });

        EventManager.SubGameOver(goc =>
        {
            curCoolTime = 0f;
            isInteractionAble = true;
        });
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
        Player user = NetworkManager.instance.User;
        if (user == null) return;

        this.isOpen = isOpen;
        SendManager.Instance.Send("SPAWNER_OPEN", new OpenPanelVO(area, missionType, user.CurTeam, id, isOpen, user.CanEnemyGathering));

        if(!isOpen)
        {
            SendManager.Instance.Send("");
            SendManager.Instance.Send("MISSION", new ItemSpawnerVO(id, user.socketId, MissionType, user.CurTeam));
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
