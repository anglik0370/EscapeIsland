using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBtn : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private ItemStorage storage;
    private Transform playerTrm;
    private Transform storageTrm;

    private Inventory inventory;
    private float range;

    private Button btn;

    private void Awake() 
    {
        btn = GetComponent<Button>();

        storage = FindObjectOfType<ItemStorage>();
        storageTrm = storage.transform;

        playerTrm = player.transform;
        inventory = player.inventory;
        range = player.range;

        btn.onClick.AddListener(PickUpNearlestItem);
    }

    private void Update() 
    {
        //가까운 재련소를 찾는다(팔길이보다 멀리있으면 null이 나옴)
        if(FindNearlestRefinery() != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                OpenRefineryPanel(FindNearlestRefinery());
            });
        }
        else if(Vector2.Distance(playerTrm.position, storageTrm.position) <= player.range)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OpenStoragePanel);
        }
        else
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(PickUpNearlestItem);
        }
    }

    public void OpenStoragePanel()
    {
        StoragePanel.Instance.Open();
    }

    public void OpenRefineryPanel(Refinery refinery)
    {
        RefineryPanel.Instance.Open(refinery);
    }

    public void PickUpNearlestItem()
    {
        //모든 슬롯이 꽉차있으면 리턴
        if(inventory.IsAllSlotFull) return;

        List<ItemSpawner> spawnerList = GameManager.Instance.spawnerList;

        ItemSpawner nearlestSpawner = null;

        //켜져있는 스포너 하나를 찾는다(비교대상이 있어야하니까)
        for(int i = 0; i < spawnerList.Count; i++)
        {
            if(spawnerList[i].IsItemSpawned)
            {
                nearlestSpawner = spawnerList[i];
                break;
            }
        }

        //켜져있는게 없으면 비교할 필요도 없다
        if(nearlestSpawner == null) return;

        //이후 나머지 켜져있는 스포너들과 거리비교
        for(int i = 0; i < spawnerList.Count; i++)
        {
            if(!spawnerList[i].IsItemSpawned) continue;

            if(Vector2.Distance(playerTrm.position, nearlestSpawner.transform.position) >
                Vector2.Distance(playerTrm.position, spawnerList[i].transform.position))
            {
                nearlestSpawner = spawnerList[i];
            }
        }

        //상호작용범위 안에 있는지 체크
        if(Vector2.Distance(playerTrm.position, nearlestSpawner.transform.position) <= range)
        {
            //있다면 넣어준다
            inventory.AddItem(nearlestSpawner.PickUpItem());
        }
    }

    public Refinery FindNearlestRefinery()
    {
        Refinery nearlestRefinery = null;

        List<Refinery> refienryList = GameManager.Instance.refineryList;

        for(int i = 0; i < refienryList.Count; i++)
        {
            //상호작용범위 안에 있는지 체크
            if(Vector2.Distance(playerTrm.position, refienryList[i].transform.position) <= range)
            {
                if(nearlestRefinery == null)
                {   
                    //없으면 하나 넣어주고
                    nearlestRefinery = refienryList[i];
                }
                else
                {
                    //있으면 거리비교
                    if(Vector2.Distance(playerTrm.position, nearlestRefinery.transform.position) >
                        Vector2.Distance(playerTrm.position, refienryList[i].transform.position))
                    {
                        nearlestRefinery = refienryList[i];
                    }
                }
            }
        }

        return nearlestRefinery;
    }
}
