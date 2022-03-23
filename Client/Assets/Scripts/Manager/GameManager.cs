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

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        itemList = Resources.LoadAll<ItemSO>("ItemSO/").ToList();
        spawnerList = GameObject.FindObjectsOfType<ItemSpawner>().ToList();
        refineryList = GameObject.FindObjectsOfType<ItemConverter>().ToList();
    }

    public ItemSO FindItemFromItemId(int id)
    {
        return itemList.Find(x => x.itemId == id);
    }
}
