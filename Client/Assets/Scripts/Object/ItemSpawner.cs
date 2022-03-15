using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
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

        EventManager.SubTimeChange(isLight =>
        {
            if (!isLight)
            {
                SpawnItem();
            }
        });

        EventManager.SubGameOver(gameOverCase =>
        {
            DeSpawnItem();
        });
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Sprite GetItemSprite()
    {
        return poolObj.GetSprite();
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

    public ItemSO PickUpItem()
    {
        //플레이어에서 검사하니까 여기선 검사할 필요 없다
        DeSpawnItem();

        return item;
    }
}
