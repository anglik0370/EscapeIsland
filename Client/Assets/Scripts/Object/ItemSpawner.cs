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

    public Action LobbyCallback => () => { };
    //public Action IngameCallback => () => SpawnerManager.Instance.PickUpSpawnerItem(this);
    public Action IngameCallback => () =>
    {
        if(missionType == MissionType.None)
        {
            SpawnerManager.Instance.PickUpSpawnerItem(this);
        }
        else if(missionType == MissionType.Charge)
        {
            MissionPanel.Instance.Open(missionType, GetComponent<ItemCharger>());
        }
        else
        {
            MissionPanel.Instance.Open(missionType);
            SendManager.Instance.StartMission(id,MissionType);
        }
    };

    public int id;

    private float maxCoolTime = 60f;
    private float curCoolTime = 0f;

    public bool isInteractionAble = true;

    //[SerializeField]
    //private ItemSO item;
    //[SerializeField]
    //private PoolObj poolObj;

    //public bool IsItemSpawned => poolObj.gameObject.activeSelf;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        //poolObj = Instantiate(poolObj, transform);

        //poolObj.SetPosition(transform.position);
        //poolObj.SetSprite(item.itemSprite);
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

    //private void Start()
    //{
    //    EventManager.SubEnterRoom(p =>
    //    {
    //        DeSpawnItem();
    //    });

    //    EventManager.SubGameStart(p =>
    //    {
    //        SpawnItem();
    //    });

    //    EventManager.SubGameOver(goc =>
    //    {
    //        DeSpawnItem();
    //    });

    //    EventManager.SubTimeChange(isLight =>
    //    {
    //        if (!isLight)
    //        {
    //            SpawnItem();
    //        }
    //    });
    //}
    public void StartTimer()
    {
        curCoolTime = maxCoolTime;
        isInteractionAble = false;
    }

    public float GetFillCoolTime()
    {
        return curCoolTime / maxCoolTime;
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Transform GetInteractionTrm()
    {
        return interactionTrm == null ? transform : interactionTrm;
    }

    public Sprite GetSprite()
    {
        //return poolObj.GetSprite();
        return sr != null ? sr.sprite : null;
    }

    public bool GetFlipX()
    {
        //return poolObj.GetFlipX();
        return sr != null ? sr.flipX : true;
    }

    //public void SpawnItem()
    //{
    //    if (poolObj.gameObject.activeSelf) return;

    //    poolObj.SetActive(true);
    //}

    //public void DeSpawnItem()
    //{
    //    if (!poolObj.gameObject.activeSelf) return;

    //    poolObj.SetActive(false);
    //}

    //public ItemSO GetItem()
    //{
    //    return item;
    //}
}
