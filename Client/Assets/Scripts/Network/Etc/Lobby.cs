using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Popup
{
    public Button refreshBtn;
    public Button createPopupOpenBtn;
    public Button exitBtn;

    [Header("CreateRoomPopup°ü·Ã")]
    public CanvasGroup createRoomPopup;
    public InputField roomNameInput;
    public Button createRoomBtn;
    public Button cancelBtn;

    private void Start()
    {
        refreshBtn.onClick.AddListener(() =>
        {

        });
        createPopupOpenBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(true);
        });
        exitBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.InitData();
            SocketClient.instance.InitWebSocket();
            PopupManager.instance.CloseAndOpen("connect");
        });

        createRoomBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.CreateRoom(roomNameInput.text,8);
        });
        cancelBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(false);
        });
    }

    public void OpenCreateRoomPopup(bool on)
    {
        createRoomPopup.alpha = on ? 1f : 0f;
        createRoomPopup.interactable = on;
        createRoomPopup.blocksRaycasts = on;
    }
}
