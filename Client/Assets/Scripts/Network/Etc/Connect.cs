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
                //���� : �ʼ� ���� ����ν� �� �����ϴ�.
                return;
            }

            SocketClient.instance.ConnectSocket(ipInput.text, portInput.text);
            //�α��� â���� �Ѿ����
            PopupManager.instance.ClosePopup();
            PopupManager.instance.OpenPopup("login");
        });
    }
}
