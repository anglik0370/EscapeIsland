using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Obsolete("���� �ʴ� Ŭ����")]
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
                //���� : �ʼ� ���� ����ν� �� �����ϴ�.
                PopupManager.instance.OpenPopup("alert", "�ʼ� ���� ����ν� �� �����ϴ�.");
                return;
            }

            //SocketClient.instance.ConnectSocket();
            //�α��� â���� �Ѿ����
            PopupManager.instance.CloseAndOpen("login");
        });
    }
}
