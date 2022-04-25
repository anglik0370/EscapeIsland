using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : Popup
{
    public InputField nameInput;

    public Button connectBtn;

    private void Start()
    {
        UIManager.Instance.OnEndEdit(nameInput, connectBtn.onClick);

        connectBtn.onClick.AddListener(() =>
        {
            //�α��� ���� �� �κ�� �̵��ؾ���
            SendManager.Instance.Login(nameInput.text);
            nameInput.text = "";

            SendManager.Instance.ReqRoomRefresh();
        });
    }
}
