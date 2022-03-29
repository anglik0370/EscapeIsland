using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopup : Popup
{
    public Button refreshBtn;
    public Button createPopupOpenBtn;
    public Button joinPopupOpenBtn;
    public Button exitBtn;

    public Transform roomParent;

    [Header("CreateRoomPopup����")]
    public CanvasGroup createRoomPopup;
    public InputField roomNameInput;
    public Button createRoomBtn;
    public Button cancelBtn;

    [Header("JoinRoom")]
    public CanvasGroup joinRoomPopup;
    public InputField joinRoomNameInput;
    public Button joinBtn;
    public Button joinCancelBtn;

    public Slider userNumslider;
    public Text userNumtext;

    public Slider kidnapperNumSlider;
    public Text kidnapperNumText;

    public Toggle testToggle;

    private void Start()
    {
        refreshBtn.onClick.AddListener(() =>
        {
            SendManager.Instance.ReqRoomRefresh();
        });
        createPopupOpenBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(true);
        });
        exitBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.ExitRoom();
            //SocketClient.instance.InitWebSocket();
            //PopupManager.instance.CloseAndOpen("ingame");
            PopupManager.instance.CloseAndOpen("login");
        });

        createRoomBtn.onClick.AddListener(() =>
        {
            SendManager.Instance.CreateRoom(roomNameInput.text,0,(int)userNumslider.value,(int)kidnapperNumSlider.value,testToggle.isOn);
            OpenCreateRoomPopup(false);
        });
        cancelBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(false);
        });

        joinPopupOpenBtn.onClick.AddListener(() =>
        {
            OpenJoinRoomPopup(true);
        });

        joinCancelBtn.onClick.AddListener(() => OpenJoinRoomPopup(false));

        joinBtn.onClick.AddListener(() =>
        {
            //JoinRoom 보내 nameInput text로 
            SendManager.Instance.SendFindRoom(joinRoomNameInput.text);
            OpenJoinRoomPopup(false);
        });


        userNumslider.onValueChanged.AddListener(x =>
        {
            userNumtext.text = $"{(int)x}";
        });

        kidnapperNumSlider.onValueChanged.AddListener(x =>
        {
            kidnapperNumText.text = $"{(int)x}";
        });
    }

    public void OpenCreateRoomPopup(bool on)
    {
        CgOpenAndClose(createRoomPopup, on);

        if(!on)
        {
            roomNameInput.text = "";
            userNumslider.value = 5;
            kidnapperNumSlider.value = 1;
            testToggle.isOn = false;
        }
    }

    public void OpenJoinRoomPopup(bool on)
    {
        CgOpenAndClose(joinRoomPopup, on);

        if(!on)
        {
            joinRoomNameInput.text = "";
        }
    }

    public void CgOpenAndClose(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }
}
