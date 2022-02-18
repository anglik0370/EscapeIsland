using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBtn : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField]
    private Player player;
    private ItemStorage storage;
    private Transform playerTrm;
    private Transform storageTrm;

    [Header("버튼 스프라이트")]
    [SerializeField]
    private Sprite interactionSprite;
    [SerializeField]
    private Sprite killSprite;
    [SerializeField]
    private Sprite PickUpSprite;

    [Header("강조 오브젝트")]
    [SerializeField]
    private ObjectAccent accent;

    private Inventory inventory;
    private float range;

    private Button btn;
    private Image image;

    public bool gameStart;


    private void Awake() 
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();

        storage = FindObjectOfType<ItemStorage>();
        storageTrm = storage.transform;

        //playerTrm = player.transform;
        //inventory = player.inventory;
        //range = player.range;

        btn.onClick.AddListener(PickUpNearlestItem);
    }

    private void Update() 
    {
        if(gameStart)
        {
            if (NetworkManager.instance.IsKidnapper() && TimeHandler.Instance.EndOfVote() && FindNearlestPlayer() != null)
            {
                image.sprite = killSprite;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(KillPlayer);

                accent.Enable(FindNearlestPlayer().GetSprite(), FindNearlestPlayer().GetPos());
            }
            else if (FindNearlestRefinery() != null)
            {
                image.sprite = interactionSprite;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    OpenRefineryPanel(FindNearlestRefinery());
                });

                accent.Enable(FindNearlestRefinery().GetSprite(), FindNearlestRefinery().GetPos());
            }
            else if (Vector2.Distance(playerTrm.position, storageTrm.position) <= player.range)
            {
                image.sprite = interactionSprite;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OpenStoragePanel);

                accent.Disable();
            }
            else
            {
                image.sprite = PickUpSprite;

                if(FindNearlestSpawner() != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(PickUpNearlestItem);

                    accent.Enable(FindNearlestSpawner().GetItemSprite(), FindNearlestSpawner().GetPos());
                }
                else
                {
                    accent.Disable();
                }
            }
        }
        
    }

    public void Init(Player p)
    {
        player = p;

        playerTrm = p.transform;
        inventory = p.inventory;
        range = p.range;
    }

    public void KillPlayer()
    {
        Player targetPlayer = FindNearlestPlayer();

        targetPlayer.SetDead();

        NetworkManager.instance.Kill(targetPlayer);
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

        ItemSpawner nearlestSpawner = FindNearlestSpawner();

        //스포너가 없다면 리턴
        if(nearlestSpawner == null) return;

        //있다면 넣어준다
        NetworkManager.instance.GetItem(nearlestSpawner.id);
        inventory.AddItem(nearlestSpawner.PickUpItem());
    }

    public ItemSpawner FindNearlestSpawner()
    {
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

        //켜져있는게 없으면 null 리턴
        if(nearlestSpawner == null) return null;

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
            //안에 있다면 스포너 리턴
            return nearlestSpawner;
        }
        else
        {
            //거리 밖이라면 null리턴
            return null;
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

    public Player FindNearlestPlayer()
    {
        Player p = null;

        List<Player> playerList = NetworkManager.instance.GetPlayerList();

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].isDie) continue;

            if (Vector2.Distance(playerTrm.position, playerList[i].transform.position) <= range)
            {
                if(p == null)
                {
                    p = playerList[i];
                }
                else
                {
                    if (Vector2.Distance(playerTrm.position, p.transform.position) >
                        Vector2.Distance(playerTrm.position, playerList[i].transform.position))
                    {
                        p = playerList[i];
                    }
                }
            }
        }

        return p;
    }
}
