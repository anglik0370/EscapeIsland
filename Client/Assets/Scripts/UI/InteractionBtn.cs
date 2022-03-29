using System;
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
    [Header("아무것도 안할 때 SO")]
    [SerializeField]
    private InteractionSO nothingSO;
    [Header("게임 시작 SO")]
    [SerializeField]
    private InteractionSO gameStartSO;

    [Header("텍스트")]
    [SerializeField]
    private Text txt;

    [Header("이미지")]
    [SerializeField]
    private Image btnImg;
    [SerializeField]
    private Image coolTimeImg;

    [Header("강조 오브젝트")]
    [SerializeField]
    private ObjectAccent accent;

    [Header("현재 상태")]
    [SerializeField]
    private InteractionCase state;

    private IInteractionObject proximateObj;

    private Button btn;
    private Image image;

    [SerializeField]
    private bool isGameStart;
    [SerializeField]
    private bool isEnterRoom;

    private void Awake() 
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();

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
    }

    private void Update() 
    {
        if (!isEnterRoom) return;

        proximateObj = GameManager.Instance.GetProximateObject();
        
        if(proximateObj != null)
        {
            print(proximateObj.GetTrm().gameObject.name);

            if (!isGameStart)
            {
                UpdateBtnState(proximateObj.LobbyHandlerSO);
                UpdateBtnCallback(proximateObj.LobbyCallback);
            }
            else
            {
                UpdateBtnState(proximateObj.InGameHandlerSO);
                UpdateBtnCallback(proximateObj.IngameCallback);
            }
        }
        else
        {
            if(!isGameStart)
            {
                UpdateBtnState(gameStartSO);
                UpdateBtnCallback(() => SendManager.Instance.GameStart());
            }
            else
            {
                UpdateBtnState(nothingSO);
                UpdateBtnCallback(() => { });
            }
        }

        UpdateCoolTimeImage();
    }

    private void UpdateBtnState(InteractionSO so)
    {
        state = so.interactoinCase;

        image.sprite = so.btnSprite;
        txt.text = so.btnText;
    }

    private void UpdateBtnCallback(Action Callback)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => Callback?.Invoke());
    }

    private void UpdateCoolTimeImage()
    {
        if (state == InteractionCase.GameStart)
        {
            coolTimeImg.fillAmount = PlayerManager.Instance.AmIMaster() ? 0f : 1f;
            btnImg.raycastTarget = PlayerManager.Instance.AmIMaster();
        }
        else if (state == InteractionCase.KillPlayer)
        {
            coolTimeImg.fillAmount = TimeHandler.Instance.CurKillCoolTime / TimeHandler.Instance.KillCoolTime;
            btnImg.raycastTarget = (TimeHandler.Instance.CurKillCoolTime / TimeHandler.Instance.KillCoolTime) <= 0;
        }
        else if (state == InteractionCase.Nothing)
        {
            coolTimeImg.fillAmount = 1f;
            btnImg.raycastTarget = false;
        }
        else
        {
            coolTimeImg.fillAmount = 0f;
            btnImg.raycastTarget = true;
        }
    }
}