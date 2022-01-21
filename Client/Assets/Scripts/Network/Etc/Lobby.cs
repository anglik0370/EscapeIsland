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
            OpenCreateRoomPopup(false);
        });

        createRoomBtn.onClick.AddListener(() =>
        {

        });
        cancelBtn.onClick.AddListener(() =>
        {

        });
    }

    public void OpenCreateRoomPopup(bool on)
    {
        createRoomPopup.alpha = on ? 1f : 0f;
        createRoomPopup.interactable = on;
        createRoomPopup.blocksRaycasts = on;
    }
}
