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
    [Header("상호작용 관련 SO")]
    public List<InteractionHandlerSO> interactionCaseList = new List<InteractionHandlerSO>();
    private Dictionary<InteractionCase, InteractionHandlerSO> interactionDic = new Dictionary<InteractionCase, InteractionHandlerSO>();

    [Header("텍스트")]
    [SerializeField]
    private Text txt;

    [Header("쿨타임 이미지")]
    [SerializeField]
    private Image coolTimeImg;

    [Header("강조 오브젝트")]
    [SerializeField]
    private ObjectAccent accent;

    [Header("현재 상태")]
    [SerializeField]
    private InteractionCase state;

    private Button btn;
    private Image image;

    [SerializeField]
    private bool isGameStart;
    [SerializeField]
    private bool isEnterRoom;

    [Header("현재 가장 가까운 오브젝트")]
    [SerializeField]
    private IInteractionObject proxiamteObj;

    //메모리 절약을 위한 변수들
    private Player player;
    private ItemConverter converter;
    private ItemStorage storage;
    private LogTable table;
    private ItemSpawner spawner;
    private DeadBody deadBody;

    private void Awake() 
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();

        //playerTrm = player.transform;
        //inventory = player.inventory;
        //range = player.range;

        txt.text = string.Empty;

        isGameStart = false;
        isEnterRoom = false;
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            isEnterRoom = true;
        });

        EventManager.SubGameStart(p =>
        {
            isGameStart = true;
        });

        EventManager.SubExitRoom(() =>
        {
            isGameStart = false;
            isEnterRoom = false;
        });

        EventManager.SubBackToRoom(() =>
        {
            isGameStart = false;
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
        if (!isEnterRoom) return;

        if(!isGameStart)
        {
            if (MeetManager.Instance.GetTableInRange(out table))
            {
                //여긴 캐릭터 선택하는 곳
                state = InteractionCase.SelectCharacter;
                proxiamteObj = table;
            }
            else
            {
                //시작버튼으로 바꿔준다
                state = InteractionCase.GameStart;
                proxiamteObj = null;
            }
        }
        else //죽어서도 버튼이 바뀌기는 해야한다
        {
            if (PlayerManager.Instance.AmIKidnapper() && PlayerManager.Instance.FindProximatePlayer(out player))
            {
                //여긴 킬하는곳
                state = InteractionCase.KillPlayer;
                proxiamteObj = player;
            }
            else if (ConverterManager.Instance.FindProximateConverter(out converter))
            {
                //여긴 제련소 여는곳
                state = InteractionCase.OpenConverter;
                proxiamteObj = converter;
            }
            else if (StorageManager.Instance.GetStorageInRange(out storage))
            {
                //여긴 저장소 여는 곳
                state = InteractionCase.OpenStorage;
                proxiamteObj = storage;
            }
            else if (MeetManager.Instance.GetTableInRange(out table))
            {
                //여긴 긴급회의 하는곳
                state = InteractionCase.EmergencyMeeting;
                proxiamteObj = table;
            }
            else if (SpawnerManager.Instance.FindProximateSpawner(out spawner))
            {
                //여긴 아이템 줍는곳
                state = InteractionCase.PickUpItem;
                proxiamteObj = spawner;
            }
            else if (DeadBodyManager.Instance.FindProximateDeadBody(out deadBody))
            {
                //여긴 주변 시체 신고하는곳
                state = InteractionCase.ReportDeadbody;
                proxiamteObj = deadBody;
            }
            else
            {
                //여긴 아무것도 아닌곳
                state = InteractionCase.Nothing;
                proxiamteObj = null;
            }
        };

        SetButtonFromState();
    }

    private void SetButtonFromState()
    {
        #region CoolTimeImg 처리

        if (state == InteractionCase.GameStart)
        {
            coolTimeImg.fillAmount = PlayerManager.Instance.AmIMaster() ? 0f : 1f;
            coolTimeImg.raycastTarget = !PlayerManager.Instance.AmIMaster();
        }
        else if (state == InteractionCase.KillPlayer)
        {
            coolTimeImg.fillAmount = TimeHandler.Instance.CurKillCoolTime / TimeHandler.Instance.KillCoolTime;
            coolTimeImg.raycastTarget = (TimeHandler.Instance.CurKillCoolTime / TimeHandler.Instance.KillCoolTime) != 0;
        }
        else if (state == InteractionCase.Nothing)
        {
            coolTimeImg.fillAmount = 1f;
            coolTimeImg.raycastTarget = true;
        }
        else
        {
            coolTimeImg.fillAmount = 0f;
            coolTimeImg.raycastTarget = false;
        }

        #endregion

        txt.text = interactionDic[state].btnText;

        image.sprite = interactionDic[state].btnSprite;

        if(proxiamteObj == null)
        {
            accent.Disable();
        }
        else
        {
            accent.Enable(proxiamteObj.GetSprite(), proxiamteObj.GetTrm(), proxiamteObj.GetFlipX());
        }

        if(proxiamteObj == null)
        {
            if(!isGameStart)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => SendManager.Instance.GameStart());
            }
            else
            {
                btn.onClick.RemoveAllListeners();
            }
        }
        else
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => proxiamteObj.Callback?.Invoke(!isGameStart));
        }
    }
}