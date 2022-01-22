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
            //로그인 수행 후 로비로 이동해야함
            NetworkManager.instance.Login(nameInput.text);
            nameInput.text = "";
        });
    }
}
