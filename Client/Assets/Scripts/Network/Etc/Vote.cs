using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vote : Popup
{
    public Button chatBtn;
    public CanvasGroup chatPanel;
    
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

        chatBtn.onClick.AddListener(() =>
        {
            CanvasOpenAndClose(chatPanel, !chatPanel.interactable);
        });
    }

    public void CanvasOpenAndClose(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }

    public void AddTestText(string msg)
    {
        testText.text += msg;
    }
}
