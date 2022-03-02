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
    public List<DeadBody> deadBodyList = new List<DeadBody>();

    public DeadBody deadBodyPrefab;

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

        PoolManager.CreatePool<DeadBody>(deadBodyPrefab.gameObject, transform, 5);
    }

    private void Start()
    {
        EventManager.SubGameStart(p =>
        {
            p.inventory = FindObjectOfType<Inventory>();
        });
    }

    public ItemSO FindItemFromItemId(int id)
    {
        return itemList.Find(x => x.itemId == id);
    }

    public void AddItemInStorage(ItemSO so)
    {
        storagePanel.AddItem(so);
    }

    public void ClearDeadBody()
    {
        if (deadBodyList.Count == 0) return;

        foreach (DeadBody deadBody in deadBodyList)
        {
            if (!deadBody.gameObject.activeSelf)
            {
                continue;
            }

            deadBody.gameObject.SetActive(false);
        }

        deadBodyList.Clear();
    }
}
