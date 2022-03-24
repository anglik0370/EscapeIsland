using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InteractionCase
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
    [Header("하나밖에 없는 오브젝트")]
    [SerializeField]
    private Player player;

    [Header("상호작용 관련 SO")]
    public List<InteractionHandlerSO> interactionCaseList = new List<InteractionHandlerSO>();
    private Dictionary<InteractionCase, InteractionHandlerSO> interactionDic = new Dictionary<InteractionCase, InteractionHandlerSO>();

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
    private InteractionCase state;

    private Inventory inventory;
    private float range;

    private Button btn;
    private Image image;

    public bool isGameStart;


    private void Awake() 
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();

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
                case InteractionCase.Nothing:
                    break;
                case InteractionCase.KillPlayer:
                    KillPlayer();
                    break;
                case InteractionCase.OpenConverter:
                    ConvertPanel.Instance.Open(FindNearlestConverter());
                    break;
                case InteractionCase.OpenStorage:
                    StoragePanel.Instance.Open();
                    break;
                case InteractionCase.EmergencyMeeting:
                    MeetManager.Instance.Meet(true);
                    break;
                case InteractionCase.ReportDeadbody:
                    DeadBodyManager.Instance.ReportProximateDeadbody();
                    break;
                case InteractionCase.PickUpItem:
                    SpawnerManager.Instance.PickUpProximateSpawnerItem();
                    break;
                case InteractionCase.GameStart:
                    NetworkManager.instance.GameStart();
                    break;
                case InteractionCase.SelectCharacter:
                    CharacterSelectPanel.Instance.Open();
                    break;
            }
        });

        for (int i = 0; i < interactionCaseList.Count; i++)
        {
            InteractionCase interactionCase = (InteractionCase)i;

            foreach (InteractionHandlerSO handler in interactionCaseList)
            {
                if(handler.interactoinCase == interactionCase)
                {
                    interactionDic.Add((InteractionCase)i, handler);
                }
            }
        }
    }

    private void Update() 
    {
        if (player == null) return;//플레이어가 없으면 방에 안들어온거니까 리턴

        if (MeetManager.Instance.GetTableInRange() != null)
        {
            //여긴 캐릭터 선택하는 곳
            state = InteractionCase.SelectCharacter;
            accent.Enable(MeetManager.Instance.GetTableInRange().GetSprite(), 
                MeetManager.Instance.GetTableInRange().GetTrm());
        }
        else
        {
            //시작버튼으로 바꿔준다
            state = InteractionCase.GameStart;
            accent.Disable();
        }

        if (isGameStart) //죽어서도 버튼이 바뀌기는 해야한다
        {
            if (player.isKidnapper && FindNearlestPlayer() != null)
            {
                //여긴 킬하는곳
                state = InteractionCase.KillPlayer;
                accent.Enable(FindNearlestPlayer().GetSprite(), FindNearlestPlayer().GetTrm(), FindNearlestPlayer().GetFlip());
            }
            else if (FindNearlestConverter() != null)
            {
                //여긴 제련소 여는곳
                state = InteractionCase.OpenConverter;
                accent.Enable(FindNearlestConverter().GetSprite(), FindNearlestConverter().GetTrm());
            }
            else if (StorageManager.Instance.GetStorageInRange() != null)
            {
                //여긴 저장소 여는 곳
                state = InteractionCase.OpenStorage;
                accent.Enable(StorageManager.Instance.GetStorageInRange().GetSprite(),
                    StorageManager.Instance.GetStorageInRange().GetTrm());
            }
            else if (MeetManager.Instance.GetTableInRange() != null)
            {
                //여긴 긴급회의 하는곳
                state = InteractionCase.EmergencyMeeting;
                accent.Enable(MeetManager.Instance.GetTableInRange().GetSprite(),
                    MeetManager.Instance.GetTableInRange().GetTrm());
            }
            else if (SpawnerManager.Instance.FindProximateSpawner() != null)
            {
                //여긴 아이템 줍는곳
                state = InteractionCase.PickUpItem;
                accent.Enable(SpawnerManager.Instance.FindProximateSpawner().GetSprite(),
                    SpawnerManager.Instance.FindProximateSpawner().GetTrm());
            }
            else if (DeadBodyManager.Instance.FindProximateDeadBody() != null)
            {
                //여긴 주변 시체 신고하는곳
                state = InteractionCase.ReportDeadbody;
                accent.Enable(DeadBodyManager.Instance.FindProximateDeadBody().GetSprite(), 
                    DeadBodyManager.Instance.FindProximateDeadBody().GetTrm());
            }
            else
            {
                //여긴 아무것도 아닌곳
                state = InteractionCase.Nothing;
                accent.Disable();
            }

            //state에 따라 버튼 처리
            SetButtonFromState();
        };
    }

    private void SetButtonFromState()
    {
        if(state == InteractionCase.GameStart)
        {
            if(player.master)
            {
                btn.interactable = true;
            }
            else
            {
                btn.interactable = false;
            }
        }
        else
        {
            btn.interactable = true;
        }

        image.sprite = interactionDic[state].btnSprite;

        txt.text = interactionDic[state].btnText;

        coolTimeCvs.alpha = interactionDic[state].useCoolTimeImg || player.isDie ? 1 : 0;
        coolTimeImg.IsFill = interactionDic[state].coolTimeImgFill || player.isDie;
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
}