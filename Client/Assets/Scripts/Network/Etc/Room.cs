using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : Popup
{
    public Button startBtn;
    public Button exitBtn;

    public JoyStick roomJoyStick;

    private void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.GameStartBtn();
        });

        exitBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.ExitRoomSend();
        });
    }
}
