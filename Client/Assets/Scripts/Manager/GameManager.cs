using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Transform playerPrefabParent;
    private List<Player> playerList = new List<Player>();

    public List<ItemSpawner> spawnerList = new List<ItemSpawner>();
    public List<ItemConverter> refineryList = new List<ItemConverter>();
    public List<ItemSO> itemList = new List<ItemSO>();
    public List<DeadBody> deadBodyList = new List<DeadBody>();

    public DeadBody deadBodyPrefab;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        itemList = Resources.LoadAll<ItemSO>("ItemSO/").ToList();
        spawnerList = GameObject.FindObjectsOfType<ItemSpawner>().ToList();
        refineryList = GameObject.FindObjectsOfType<ItemConverter>().ToList();

        PoolManager.CreatePool<DeadBody>(deadBodyPrefab.gameObject, transform, 5);
    }

    public ItemSO FindItemFromItemId(int id)
    {
        return itemList.Find(x => x.itemId == id);
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
