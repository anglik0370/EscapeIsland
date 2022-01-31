using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Popup
{
    public Button refreshBtn;
    public Button createPopupOpenBtn;
    public Button exitBtn;

    public Transform roomParent;

    [Header("CreateRoomPopup°ü·Ã")]
    public CanvasGroup createRoomPopup;
    public InputField roomNameInput;
    public Button createRoomBtn;
    public Button cancelBtn;
    public Slider userNumslider;
    public Text userNumtext;

    private void Start()
    {
        NetworkManager.instance.roomParent = roomParent;

        refreshBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.ReqRoomRefresh();
        });
        createPopupOpenBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(true);
        });
        exitBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.SetIngameCanvas(false);
            NetworkManager.instance.InitData();
            SocketClient.instance.InitWebSocket();
            //PopupManager.instance.CloseAndOpen("ingame");
            PopupManager.instance.CloseAndOpen("connect");
        });

        createRoomBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(false);
            NetworkManager.instance.CreateRoom(roomNameInput.text,0,8);
        });
        cancelBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(false);
        });
        userNumslider.onValueChanged.AddListener(x =>
        {
            userNumtext.text = $"Players : {(int)x}";
        });
    }

    public void OpenCreateRoomPopup(bool on)
    {
        createRoomPopup.alpha = on ? 1f : 0f;
        createRoomPopup.interactable = on;
        createRoomPopup.blocksRaycasts = on;

        if(!on)
        {
            userNumslider.value = 8;
        }
    }
}
