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
        });
    }

    public bool FindProximateSpawner(out ItemSpawner temp)
    {
        ItemSpawner spawner = null;

        for (int i = 0; i < spawnerList.Count; i++)
        {
            //상호작용범위 안에 있는지 체크
            if (Vector2.Distance(player.GetTrm().position, spawnerList[i].transform.position) <= player.range)
            {
                //꺼져있으면 다음걸로 넘어간다
                if (!spawnerList[i].IsItemSpawned) continue;

                if (spawner == null) spawner = spawnerList[i];

                //거리비교
                if (Vector2.Distance(player.GetTrm().position, spawner.transform.position) >
                        Vector2.Distance(player.GetTrm().position, spawnerList[i].transform.position))
                {
                    spawner = spawnerList[i];
                }
            }
        }

        temp = spawner;

        return temp != null;
    }

    public void PickUpProximateSpawnerItem()
    {
        //모든 슬롯이 꽉차있으면 리턴
        if (player.inventory.IsAllSlotFull) return;

        FindProximateSpawner(out ItemSpawner nearlestSpawner);

        //스포너가 없다면 리턴
        if (nearlestSpawner == null) return;

        //있다면 넣어준다
        NetworkManager.instance.GetItem(nearlestSpawner.id);
        player.inventory.AddItem(nearlestSpawner.PickUpItem());
    }
}
