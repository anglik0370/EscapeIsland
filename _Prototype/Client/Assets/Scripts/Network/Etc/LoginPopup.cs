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
            SendManager.Instance.Send("LOGIN", new LoginVO(nameInput.text, 0));
            nameInput.text = "";

            SendManager.Instance.Send("ROOM_REFRESH_REQ");
        });
    }
}
