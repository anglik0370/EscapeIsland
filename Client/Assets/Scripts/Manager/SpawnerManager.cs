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

    public ItemSpawner FindProximateSpawner()
    {
        ItemSpawner spawner = null;

        for (int i = 0; i < spawnerList.Count; i++)
        {
            //��ȣ�ۿ���� �ȿ� �ִ��� üũ
            if (Vector2.Distance(player.GetTrm().position, spawnerList[i].transform.position) <= player.range)
            {
                //���������� �����ɷ� �Ѿ��
                if (!spawner.IsItemSpawned) continue;

                //�Ÿ���
                if (Vector2.Distance(player.GetTrm().position, spawner.transform.position) >
                        Vector2.Distance(player.GetTrm().position, spawnerList[i].transform.position))
                {
                    spawner = spawnerList[i];
                }
            }
        }

        return spawner;
    }

    public void PickUpProximateSpawnerItem()
    {
        //��� ������ ���������� ����
        if (player.inventory.IsAllSlotFull) return;

        ItemSpawner nearlestSpawner = FindProximateSpawner();

        //�����ʰ� ���ٸ� ����
        if (nearlestSpawner == null) return;

        //�ִٸ� �־��ش�
        NetworkManager.instance.GetItem(nearlestSpawner.id);
        player.inventory.AddItem(nearlestSpawner.PickUpItem());
    }
}
