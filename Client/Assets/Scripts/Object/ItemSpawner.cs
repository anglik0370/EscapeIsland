using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemSO item;
    public PoolObj poolObj;

    private void Awake() 
    {
        poolObj = Instantiate(poolObj, transform);
        poolObj.SetActive(false);
    }

    public void SetItem(ItemSO item)
    {
        this.item = item;
    }

    public void SpawnItem()
    {
        poolObj.SetPosition(transform.position);
        poolObj.SetSprite(item.itemSprite);

        poolObj.SetActive(true);
    }
}
