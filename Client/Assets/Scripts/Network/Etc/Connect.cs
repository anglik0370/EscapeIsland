using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connect : Popup
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
                return;
            }

            SocketClient.instance.ConnectSocket(ipInput.text, portInput.text);
            //로그인 창으로 넘어가야함
            PopupManager.instance.ClosePopup();
            PopupManager.instance.OpenPopup("login");
        });
    }
}
