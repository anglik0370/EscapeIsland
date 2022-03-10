using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum InteractionState
{
    Nothing,
    KillPlayer,
    OpenRefienry,
    OpenStorage,
    EmergencyMeeting,
    ReportDeadbody,
    PickUpItem,
}

public class InteractionBtn : MonoBehaviour
{
    [Header("하나밖에 없는 오브젝트들")]
    [SerializeField]
    private Player player;
    private ItemStorage storage;
    private EmergencyMeetingTable meetingTable;

    [Header("버튼 스프라이트")]
    [SerializeField]
    private Sprite startSprite;
    [SerializeField]
    private Sprite interactionSprite;
    [SerializeField]
    private Sprite killSprite;
    [SerializeField]
    private Sprite pickUpSprite;
    [SerializeField]
    private Sprite emergencySprite;
    [SerializeField]
    private Sprite findDeadBodySprite;

    [Header("텍스트")]
    [SerializeField]
    private Text text;

    [Header("쿨타임 이미지")]
    [SerializeField]
    private CanvasGroup cooltimeImg;

    [Header("강조 오브젝트")]
    [SerializeField]
    private ObjectAccent accent;

    [Header("현재 상태")]
    [SerializeField]
    private InteractionState state;

    private Inventory inventory;
    private float range;

    private Button btn;
    private Image image;

    public bool isGameStart;


    private void Awake() 
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();

        storage = FindObjectOfType<ItemStorage>();
        meetingTable = FindObjectOfType<EmergencyMeetingTable>();

        //playerTrm = player.transform;
        //inventory = player.inventory;
        //range = player.range;

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        isGameStart = false;
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;

            p.inventory = FindObjectOfType<Inventory>();

            inventory = p.inventory;
            range = p.range;

            text.text = "Start";
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

            if(!p.master)
            {
                btn.interactable = false;
            }

            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(NetworkManager.instance.GameStartBtn);
        });

        EventManager.SubGameStart(p =>
        {
            isGameStart = true;

            text.text = "Help!";
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            cooltimeImg.alpha = 0f;

            btn.interactable = true;

            image.sprite = startSprite;

            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(() =>
            {
                if (player.isDie) return;

                switch (state)
                {
                    case InteractionState.KillPlayer:
                        KillPlayer();
                        break;
                    case InteractionState.OpenRefienry:
                        OpenRefineryPanel(FindNearlestRefinery());
                        break;
                    case InteractionState.OpenStorage:
                        OpenStoragePanel();
                        break;
                    case InteractionState.EmergencyMeeting:
                        meetingTable.Meeting();
                        break;
                    case InteractionState.ReportDeadbody:
                        ReportNearlestDeadbody();
                        break;
                    case InteractionState.PickUpItem:
                        PickUpNearlestItem();
                        break;
                    case InteractionState.Nothing:
                        return;
                }
            });
        });

        EventManager.SubExitRoom(() =>
        {
            isGameStart = false;
        });

        EventManager.SubBackToRoom(() =>
        {
            isGameStart = false;

            btn.onClick.RemoveAllListeners();

            if (player.master)
            {
                btn.onClick.AddListener(NetworkManager.instance.GameStartBtn);
            }
        });
    }

    private void Update() 
    {
        if (!isGameStart || player.isDie) return;
        //print(TimeHandler.Instance.EndOfVote());
        //print(FindNearlestPlayer());
        if (NetworkManager.instance.IsKidnapper() && TimeHandler.Instance.EndOfVote() && FindNearlestPlayer() != null)
        {
            //여긴 킬하는곳
            state = InteractionState.KillPlayer;

            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

            cooltimeImg.alpha = 1f;

            image.sprite = killSprite;
            accent.Enable(FindNearlestPlayer().GetSprite(), FindNearlestPlayer().GetTrm(), FindNearlestPlayer().GetFlip());
        }
        else if (FindNearlestRefinery() != null)
        {
            //여긴 제련소 여는곳
            state = InteractionState.OpenRefienry;

            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

            cooltimeImg.alpha = 0f;

            image.sprite = interactionSprite;
            accent.Enable(FindNearlestRefinery().GetSprite(), FindNearlestRefinery().GetTrm());
        }
        else if (Vector2.Distance(player.GetTrm().position, storage.GetTrm().position) <= player.range)
        {
            //여긴 저장소 여는 곳
            state = InteractionState.OpenStorage;

            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

            cooltimeImg.alpha = 0f;

            image.sprite = interactionSprite;
            accent.Disable();
        }
        else if (Vector2.Distance(player.GetTrm().position, meetingTable.GetTrm().position) <= player.range)
        {
            //여긴 긴급회의 여는 곳
            state = InteractionState.EmergencyMeeting;

            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

            cooltimeImg.alpha = 0f;

            image.sprite = emergencySprite;
            accent.Enable(meetingTable.GetSprite(), meetingTable.GetTrm());
        }
        else
        {
            if (FindNearlestSpawner() != null)
            {
                //여긴 아이템 줍는곳
                state = InteractionState.PickUpItem;

                image.sprite = pickUpSprite;
                accent.Enable(FindNearlestSpawner().GetItemSprite(), FindNearlestSpawner().GetTrm());
            }
            else if (FindNearlestDeadBody() != null)
            {
                //여긴 주변 시체 신고하는곳
                state = InteractionState.ReportDeadbody;

                image.sprite = findDeadBodySprite;
                accent.Enable(FindNearlestDeadBody().GetSprite(), FindNearlestDeadBody().GetTrm());
            }
            else
            {
                //여긴 아무것도 아닌곳
                state = InteractionState.Nothing;

                image.sprite = pickUpSprite;
                accent.Disable();
            }

            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

            cooltimeImg.alpha = 0f;
        }
    }

    public void KillPlayer()
    {
        if(!TimeHandler.Instance.isKillAble)
        {
            //킬 스택이 부족합니다 <- 메시지 표시
            UIManager.Instance.SetWarningText("아직 킬 할 수 없습니다.");
            return;
        }
        Player targetPlayer = FindNearlestPlayer();

        if (targetPlayer == null) return;

        targetPlayer.SetDead();

        TimeHandler.Instance.isKillAble = false;
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

    public void ReportNearlestDeadbody()
    {
        DeadBody nearlestDeadbody = FindNearlestDeadBody();

        if (nearlestDeadbody == null) return;

        nearlestDeadbody.Report();
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

            if(Vector2.Distance(player.GetTrm().position, nearlestSpawner.transform.position) >
                Vector2.Distance(player.GetTrm().position, spawnerList[i].transform.position))
            {
                nearlestSpawner = spawnerList[i];
            }
        }

        //상호작용범위 안에 있는지 체크
        if(Vector2.Distance(player.GetTrm().position, nearlestSpawner.transform.position) <= range)
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
            if(Vector2.Distance(player.GetTrm().position, refienryList[i].transform.position) <= range)
            {
                if(nearlestRefinery == null)
                {   
                    //없으면 하나 넣어주고
                    nearlestRefinery = refienryList[i];
                }
                else
                {
                    //있으면 거리비교
                    if(Vector2.Distance(player.GetTrm().position, nearlestRefinery.transform.position) >
                        Vector2.Distance(player.GetTrm().position, refienryList[i].transform.position))
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

            if (Vector2.Distance(player.GetTrm().position, playerList[i].transform.position) <= range)
            {
                if(p == null)
                {
                    p = playerList[i];
                }
                else
                {
                    if (Vector2.Distance(player.GetTrm().position, p.transform.position) >
                        Vector2.Distance(player.GetTrm().position, playerList[i].transform.position))
                    {
                        p = playerList[i];
                    }
                }
            }
        }

        return p;
    }

    public DeadBody FindNearlestDeadBody()
    {
        DeadBody deadBody = null;

        List<DeadBody> deadBodyList = GameManager.Instance.deadBodyList;

        for (int i = 0; i < deadBodyList.Count; i++)
        {
            //상호작용범위 안에 있는지 체크
            if (Vector2.Distance(player.GetTrm().position, deadBodyList[i].transform.position) <= range)
            {
                if (deadBody == null)
                {
                    //없으면 하나 넣어주고
                    deadBody = deadBodyList[i];
                }
                else
                {
                    //있으면 거리비교
                    if (Vector2.Distance(player.GetTrm().position, deadBody.transform.position) >
                        Vector2.Distance(player.GetTrm().position, deadBodyList[i].transform.position))
                    {
                        deadBody = deadBodyList[i];
                    }
                }
            }
        }

        return deadBody;
    }
}
