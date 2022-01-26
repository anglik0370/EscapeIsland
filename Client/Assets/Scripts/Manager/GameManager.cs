using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<ItemSpawner> spawnerList = new List<ItemSpawner>();
    public List<Refinery> refineryList = new List<Refinery>();
    public List<ItemSO> itemList = new List<ItemSO>();

    private ItemStorage itemStorage;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        itemList = Resources.LoadAll<ItemSO>("ItemSO/").ToList();
        spawnerList = GameObject.FindObjectsOfType<ItemSpawner>().ToList();
        refineryList = GameObject.FindObjectsOfType<Refinery>().ToList();
        itemStorage = FindObjectOfType<ItemStorage>();
    }

    public ItemSO FindItemFromItemId(int id)
    {
        return itemList.Find(x => x.itemId == id);
    }

    public void AddItem(ItemSO so)
    {
        itemStorage.AddItem(so);
    }
}
