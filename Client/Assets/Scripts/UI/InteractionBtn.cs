using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum InteractionState
{
    Nothing,
    KillPlayer,
    OpenConverter,
    OpenStorage,
    EmergencyMeeting,
    ReportDeadbody,
    PickUpItem,
    GameStart,
    SelectCharacter,
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
    private Text txt;

    [Header("쿨타임 이미지")]
    [SerializeField]
    private CanvasGroup coolTimeCvs;
    [SerializeField]
    private CircleFillImage coolTimeImg;

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

        txt.text = string.Empty;

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
        });

        EventManager.SubGameStart(p =>
        {
            isGameStart = true;
        });

        EventManager.SubExitRoom(() =>
        {
            isGameStart = false;
        });

        EventManager.SubBackToRoom(() =>
        {
            isGameStart = false;
        });

        btn.onClick.AddListener(() =>
        {
            if (player.isDie) return;

            switch (state)
            {
                case InteractionState.Nothing:
                    break;
                case InteractionState.KillPlayer:
                    KillPlayer();
                    break;
                case InteractionState.OpenConverter:
                    OpenRefineryPanel(FindNearlestConverter());
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
                case InteractionState.GameStart:
                    NetworkManager.instance.GameStart();
                    break;
                case InteractionState.SelectCharacter:
                    OpenCharacterSelectPanel();
                    break;
            }
        });
    }

    private void Update() 
    {
        if (player != null) //플레이어가 없으면 방에 안들어온거니까 리턴

        if (Vector2.Distance(player.GetTrm().position, meetingTable.GetTrm().position) <= player.range)
        {
            //여긴 캐릭터 선택하는 곳
            state = InteractionState.SelectCharacter;
        }
        else
        {
            //시작버튼으로 바꿔준다
            state = InteractionState.GameStart;
        }

        if (isGameStart && !player.isDie)
        {
            if (NetworkManager.instance.IsKidnapper() && FindNearlestPlayer() != null)
            {
                //여긴 킬하는곳
                state = InteractionState.KillPlayer;
            }
            else if (FindNearlestConverter() != null)
            {
                //여긴 제련소 여는곳
                state = InteractionState.OpenConverter;
            }
            else if (Vector2.Distance(player.GetTrm().position, storage.GetInteractionTrm().position) <= player.range)
            {
                //여긴 저장소 여는 곳
                state = InteractionState.OpenStorage;
            }
            else if (Vector2.Distance(player.GetTrm().position, meetingTable.GetTrm().position) <= player.range)
            {
                //여긴 긴급회의 여는 곳
                state = InteractionState.EmergencyMeeting;
            }
            else if (FindNearlestSpawner() != null)
            {
                //여긴 아이템 줍는곳
                state = InteractionState.PickUpItem;
            }
            else if (FindNearlestDeadBody() != null)
            {
                //여긴 주변 시체 신고하는곳
                state = InteractionState.ReportDeadbody;
            }
            else
            {
                //여긴 아무것도 아닌곳
                state = InteractionState.Nothing;
            }
        };

        //state에 따라 버튼 처리
        SetButtonFromState();
    }

    private void SetButtonState(Sprite btnSprite, Sprite accentSprite = null, Transform accentTrm = null, bool isAccnetFilp = false)
    {
        image.sprite = btnSprite;

        if(accentSprite == null && accentTrm == null)
        {
            accent.Disable();
        }
        else
        {
            accent.Enable(accentSprite, accentTrm, isAccnetFilp);
        }
    }

    private void SetCoolTimeImg(bool enable = false, bool isFill = false)
    {
        coolTimeCvs.alpha = enable ? 1 : 0;
        coolTimeImg.IsFill = isFill;
    }

    private void SetText(string text = "")
    {
        txt.text = text;
    }

    private void SetButtonFromState()
    {
        switch (state)
        {
            case InteractionState.Nothing:
                {
                    SetButtonState(pickUpSprite);
                    SetText();
                    SetCoolTimeImg(true, true);
                }
                break;
            case InteractionState.KillPlayer:
                {
                    SetButtonState(killSprite, FindNearlestPlayer().GetSprite(), FindNearlestPlayer().GetTrm());

                    if (TimeHandler.Instance.CurKillCoolTime > 0)
                    {
                        SetText(Mathf.Floor(TimeHandler.Instance.CurKillCoolTime).ToString());
                    }
                    else
                    {
                        SetText();
                    }

                    SetCoolTimeImg(true);
                }
                break;
            case InteractionState.OpenConverter:
                {
                    SetButtonState(interactionSprite, FindNearlestConverter().GetSprite(), FindNearlestConverter().GetTrm());
                    SetText();
                    SetCoolTimeImg();
                }
                break;
            case InteractionState.OpenStorage:
                {
                    SetButtonState(interactionSprite, storage.GetSprite(), storage.GetTrm());
                    SetText();
                    SetCoolTimeImg();
                }
                break;
            case InteractionState.EmergencyMeeting:
                {
                    SetButtonState(emergencySprite, meetingTable.GetSprite(), meetingTable.GetTrm());
                    SetText("Help!");
                    SetCoolTimeImg();
                }
                break;
            case InteractionState.ReportDeadbody:
                {
                    SetButtonState(findDeadBodySprite, FindNearlestDeadBody().GetSprite(), FindNearlestDeadBody().GetTrm());
                    SetText();
                    SetCoolTimeImg();
                }
                break;
            case InteractionState.PickUpItem:
                {
                    SetButtonState(pickUpSprite, FindNearlestSpawner().GetItemSprite(), FindNearlestSpawner().GetTrm());
                    SetText();
                    SetCoolTimeImg();
                }
                break;
            case InteractionState.GameStart:
                {
                    SetButtonState(startSprite);
                    SetText("Start");
                    SetCoolTimeImg();

                    if (player.master)
                    {
                        btn.interactable = true;
                    }
                    else
                    {
                        btn.interactable = false;
                    }
                }
                break;
            case InteractionState.SelectCharacter:
                {
                    btn.interactable = true;

                    SetButtonState(emergencySprite, meetingTable.GetSprite(), meetingTable.GetTrm());
                    SetText("Select");
                    SetCoolTimeImg();
                }
                break;
        }
    }

    public void OpenCharacterSelectPanel()
    {
        CharacterSelectPanel.Instance.Open();
    }

    public void OpenStoragePanel()
    {
        StoragePanel.Instance.Open();
    }

    public void OpenRefineryPanel(ItemConverter refinery)
    {
        ConvertPanel.Instance.Open(refinery);
    }

    public void KillPlayer()
    {
        if (!TimeHandler.Instance.isKillAble)
        {
            //킬 스택이 부족합니다 <- 메시지 표시
            UIManager.Instance.SetWarningText("아직 킬 할 수 없습니다.");
            return;
        }

        Player targetPlayer = FindNearlestPlayer();

        if (targetPlayer == null) return;

        targetPlayer.SetDead();

        TimeHandler.Instance.InitKillCool();

        NetworkManager.instance.Kill(targetPlayer);
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

    public ItemConverter FindNearlestConverter()
    {
        ItemConverter nearlestRefinery = null;

        List<ItemConverter> refienryList = GameManager.Instance.refineryList;

        for(int i = 0; i < refienryList.Count; i++)
        {
            //상호작용범위 안에 있는지 체크
            if(Vector2.Distance(player.GetTrm().position, refienryList[i].GetInteractionTrm().position) <= range)
            {
                if(nearlestRefinery == null)
                {   
                    //없으면 하나 넣어주고
                    nearlestRefinery = refienryList[i];
                }
                else
                {
                    //있으면 거리비교
                    if(Vector2.Distance(player.GetTrm().position, nearlestRefinery.GetTrm().position) >
                        Vector2.Distance(player.GetTrm().position, refienryList[i].GetTrm().position))
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