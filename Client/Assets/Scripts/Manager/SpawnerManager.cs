using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    [SerializeField]
    private List<ItemSpawner> spawnerList = new List<ItemSpawner>();
    public List<ItemSpawner> SpawnerList => spawnerList;

    private Player player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        spawnerList = FindObjectsOfType<ItemSpawner>().ToList(); 
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;

            for (int i = 0; i < spawnerList.Count; i++)
            {
                GameManager.Instance.AddInteractionObj(spawnerList[i]);
            }
        });
    }

    public void PickUpSpawnerItem(ItemSpawner spawner)
    {
        player.inventory.AddItem(spawner.GetItem());
        spawner.DeSpawnItem();
    }
}
