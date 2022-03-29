using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, IInteractionObject
{
    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => ingameHandlerSO;

    public bool CanInteraction => IsItemSpawned;

    public Action LobbyCallback => () => { };
    public Action IngameCallback => () => SpawnerManager.Instance.PickUpSpawnerItem(this);

    public int id;

    [SerializeField]
    private ItemSO item;
    [SerializeField]
    private PoolObj poolObj;

    public bool IsItemSpawned => poolObj.gameObject.activeSelf;

    private void Awake() 
    {
        poolObj = Instantiate(poolObj, transform);

        poolObj.SetPosition(transform.position);
        poolObj.SetSprite(item.itemSprite);
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            DeSpawnItem();
        });

        EventManager.SubGameStart(p =>
        {
            SpawnItem();
        });

        EventManager.SubGameOver(goc =>
        {
            DeSpawnItem();
        });

        EventManager.SubTimeChange(isLight =>
        {
            if (!isLight)
            {
                SpawnItem();
            }
        });
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
        return poolObj.GetSprite();
    }

    public bool GetFlipX()
    {
        return poolObj.GetFlipX();
    }

    public void SpawnItem()
    {
        if (poolObj.gameObject.activeSelf) return;

        poolObj.SetActive(true);
    }

    public void DeSpawnItem()
    {
        if (!poolObj.gameObject.activeSelf) return;

        poolObj.SetActive(false);
    }

    public ItemSO GetItem()
    {
        return item;
    }
}
