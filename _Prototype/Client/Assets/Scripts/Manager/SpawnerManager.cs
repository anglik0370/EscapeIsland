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

    [SerializeField]
    private List<MissionType> missionTypeList;
    [SerializeField]
    private List<float> missionCoolTimeList;
    
    private Player player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        spawnerList = FindObjectsOfType<ItemSpawner>().ToList();

        InitCoolTime();
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
        if (player.inventory.IsAllSlotFull) return;

        //player.inventory.AddItem(spawner.GetItem());
        //spawner.DeSpawnItem();
    }

    public ItemSpawner FindSpawner(int spawnerId, MissionType type)
    {
        return spawnerList.Find(spawner => spawner.id.Equals(spawnerId) && spawner.MissionType.Equals(type));
    }

    public void InitCoolTime()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            for (int j = 0; j < missionTypeList.Count; j++)
            {
                if (spawnerList[i].MissionType == missionTypeList[j])
                {
                    spawnerList[i].SetMaxCoolTime(missionCoolTimeList[j]);
                    break;
                }
            }
        }
    }
}
