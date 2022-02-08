using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vote : Popup
{
    public Text testText;

    public InputField msgInputField;
    public Button sendMsgBtn;

    private void Start()
    {
        sendMsgBtn.onClick.AddListener(() =>
        {
            if (msgInputField.text == "") return;

            NetworkManager.instance.SendChat(msgInputField.text);
            msgInputField.text = "";
        });
    }

    public void AddTestText(string msg)
    {
        testText.text += msg;
    }
}
