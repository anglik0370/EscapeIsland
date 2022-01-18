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

        poolObj.SetPosition(transform.position);
        poolObj.SetSprite(item.itemSprite);

        poolObj.SetActive(false);
    }

    public void SetItem(ItemSO item)
    {
        this.item = item;
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
        ItemSO temp = null;

        if(poolObj.gameObject.activeSelf)
        {
            poolObj.SetActive(false);
            temp = this.item;
        }

        return temp;
    }
}
