using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vote : Popup
{
    public Button chatBtn;
    public CanvasGroup chatPanel;

    [Header("CHAT")]
    public List<ChatUI> myChatList = new List<ChatUI>();
    public List<ChatUI> otherChatList = new List<ChatUI>();
    public ScrollRect chatRect;
    public Transform chatParent;
    public ChatUI myChat;
    public ChatUI otherChat;

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

    public void CreateChat(bool myChat,string name, string chatMsg, Sprite charSpr)
    {
        if(myChat)
        {
            ChatUI ui = myChatList.Find(x => !x.gameObject.activeSelf);

            if(ui == null)
            {
                ui = Instantiate(this.myChat, transform);
                myChatList.Add(ui);
            }

            ui.SetChatUI(name, chatMsg, charSpr,chatParent);
        }
        else
        {
            ChatUI ui = otherChatList.Find(x => !x.gameObject.activeSelf);

            if(ui == null)
            {
                ui = Instantiate(otherChat, transform);
                otherChatList.Add(ui);
            }

            ui.SetChatUI(name, chatMsg, charSpr,chatParent);
        }
        StartCoroutine(EndFrame());
        
    }

    IEnumerator EndFrame()
    {
        yield return new WaitForEndOfFrame();
        chatRect.verticalNormalizedPosition = 0.0f;
    }
}
