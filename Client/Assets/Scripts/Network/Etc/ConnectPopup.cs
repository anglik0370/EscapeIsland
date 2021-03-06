using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Obsolete("쓰지 않는 클래스")]
public class ConnectPopup : Popup
{
    public InputField ipInput;
    public InputField portInput;

    public Button connectBtn;

    private void Start()
    {
        connectBtn.onClick.AddListener(() =>
        {
            if(ipInput.text.Equals("") || portInput.text.Equals(""))
            {
                //에러 : 필수 값은 비워두실 수 없습니다.
                PopupManager.instance.OpenPopup("alert", "필수 값은 비워두실 수 없습니다.");
                return;
            }

            //SocketClient.instance.ConnectSocket();
            //로그인 창으로 넘어가야함
            PopupManager.instance.CloseAndOpen("login");
        });
    }
}
