using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBtn : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private Inventory inventory;
    private float range;

    private Button btn;

    private void Awake() 
    {
        btn = GetComponent<Button>();

        range = player.range;
        inventory = player.inventory;

        btn.onClick.AddListener(PickUpNearlestItem);
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

            if(Vector2.Distance(transform.position, nearlestSpawner.transform.position) >
                Vector2.Distance(transform.position, spawnerList[i].transform.position))
            {
                nearlestSpawner = spawnerList[i];
            }
        }

        //수집범위 안에 있는지 체크
        if(Vector2.Distance(transform.position, nearlestSpawner.transform.position) <= range)
        {
            //있다면 넣어준다
            inventory.AddItem(nearlestSpawner.PickUpItem());
        }
    }
}
