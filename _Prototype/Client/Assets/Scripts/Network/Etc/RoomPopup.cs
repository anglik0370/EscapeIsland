using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPopup : Popup
{
    public Button startBtn;
    public Button exitBtn;

    public JoyStick roomJoyStick;

    private void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            SendManager.Instance.GameStart();
        });

        exitBtn.onClick.AddListener(() =>
        {
            SendManager.Instance.Send("EXIT_ROOM");
        });
    }
}
