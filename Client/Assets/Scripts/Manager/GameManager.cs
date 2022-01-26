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

    private StoragePanel storagePanel;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        itemList = Resources.LoadAll<ItemSO>("ItemSO/").ToList();
        spawnerList = GameObject.FindObjectsOfType<ItemSpawner>().ToList();
        refineryList = GameObject.FindObjectsOfType<Refinery>().ToList();
        storagePanel = FindObjectOfType<StoragePanel>();
    }

    public ItemSO FindItemFromItemId(int id)
    {
        return itemList.Find(x => x.itemId == id);
    }

    public void AddItemInStorage(ItemSO so)
    {
        storagePanel.AddItem(so);
    }
}
