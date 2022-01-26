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

        SpawnItem();
    }

    public void SpawnItem()
    {
        poolObj.SetActive(true);
    }

    public void DeSpawnItem()
    {
        poolObj.SetActive(false);
    }

    public ItemSO PickUpItem()
    {
        //플레이어에서 검사하니까 여기선 검사할 필요 없다
        DeSpawnItem();

        return item;
    }
}
