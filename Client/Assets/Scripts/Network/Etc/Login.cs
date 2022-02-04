using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : Popup
{
    public InputField nameInput;

    public Button connectBtn;

    private void Start()
    {
        connectBtn.onClick.AddListener(() =>
        {
            //�α��� ���� �� �κ�� �̵��ؾ���
            NetworkManager.instance.Login(nameInput.text);
            nameInput.text = "";

            NetworkManager.instance.ReqRoomRefresh();
        });
    }
}
