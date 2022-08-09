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
    Ready,
    Altar,
}

public class InteractionBtn : MonoBehaviour
{
    [Header("아무것도 안할 때 SO")]
    [SerializeField]
    private InteractionSO nothingSO;
    [Header("게임 시작 SO")]
    [SerializeField]
    private InteractionSO gameStartSO;
    [Header("레디 SO")]
    [SerializeField]
    private InteractionSO readySO;

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
    [SerializeField]
    private BoneObjectAccent boneAccent;

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

    public bool CanTouch => btnImg.raycastTarget;

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
        if (GameManager.Instance.IsPanelOpen) return;

        //if(PlayerManager.Instance.AmIDead())
        //{
        //    UpdateBtnState(nothingSO);
        //    UpdateBtnCallback(() => { });
        //}
        //else
        //{

        //}

        proximateObj = GameManager.Instance.GetProximateObject();

        if (proximateObj != null)
        {
            if (!isGameStart)
            {
                if(proximateObj.LobbyHandlerSO == gameStartSO)
                {
                    if (PlayerManager.Instance.AmIMaster())
                    {
                        UpdateBtnState(gameStartSO);
                        UpdateBtnCallback(() => SendManager.Instance.GameStart());
                    }
                    else
                    {
                        UpdateBtnState(readySO);
                        UpdateBtnCallback(() => SendManager.Instance.Send("READY"));
                    }
                }
                else
                {
                    UpdateBtnState(proximateObj.LobbyHandlerSO);
                    UpdateBtnCallback(proximateObj.LobbyCallback);
                }
            }
            else
            {
                if (PlayerManager.Instance.Player.IsSturned)
                {
                    UpdateBtnState(nothingSO);
                    UpdateBtnCallback(() => { });
                }
                else
                {
                    UpdateBtnState(proximateObj.InGameHandlerSO);
                    UpdateBtnCallback(proximateObj.IngameCallback);
                }
            }
        }
        else
        {
            if (!isGameStart)
            {
                if (PlayerManager.Instance.AmIMaster())
                {
                    UpdateBtnState(gameStartSO);
                    UpdateBtnCallback(() => SendManager.Instance.GameStart());
                }
                else
                {
                    UpdateBtnState(readySO);
                    UpdateBtnCallback(() => SendManager.Instance.Send("READY"));
                }
            }
            else
            {
                UpdateBtnState(nothingSO);
                UpdateBtnCallback(() => { });
            }
        }

        UpdateCoolTimeImage();
        UpdateAccent();
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
        if (state == InteractionCase.Nothing)
        {
            coolTimeImg.fillAmount = 1f;
            btnImg.raycastTarget = false;
        }
        else if (state == InteractionCase.PickUpItem)
        {
            ItemSpawner spawner = proximateObj as ItemSpawner;

            coolTimeImg.fillAmount = spawner.GetFillCoolTime();
            btnImg.raycastTarget = spawner.isInteractionAble;
        }
        else if(state == InteractionCase.Altar)
        {
            coolTimeImg.fillAmount = AltarPanel.Instance.GetAmount();
            btnImg.raycastTarget = AltarPanel.Instance.IsAltarAble;
        }
        else
        {
            coolTimeImg.fillAmount = 0f;
            btnImg.raycastTarget = true;
        }
    }

    private void UpdateAccent()
    {
        if (state == InteractionCase.GameStart || state == InteractionCase.Ready || state == InteractionCase.Nothing)
        {
            accent.Disable();
        }
        else if (state == InteractionCase.KillPlayer)
        {
            accent.Disable();
        }
        else if (state == InteractionCase.ReportDeadbody)
        {
            accent.Disable();
        }
        else
        {
            accent.Enable(proximateObj.GetSprite(), proximateObj.GetTrm(), proximateObj.GetFlipX());
        }
    }
}