using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public ToggleGroup toggleGroup;
    public Transform voteParent;
    public Button voteCompleteBtn;

    public List<VoteUI> voteUIList = new List<VoteUI>();

    private void Start()
    {
        voteUIList = voteParent.GetComponentsInChildren<VoteUI>().ToList();

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

        voteCompleteBtn.onClick.AddListener(() =>
        {
            //여기서 서버에 보내줘야 한다
        });

    }

    public void CanvasOpenAndClose(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }

    public void SetVoteUI(string name, Sprite charSprite)
    {
        VoteUI ui = voteUIList.Find(x => !x.gameObject.activeSelf);

        if(ui == null)
        {
            Debug.LogError("없음");
            return;
        }

        ui.SetVoteUI(name, charSprite, toggleGroup);
    }

    public void VoteUIDisable()
    {
        voteUIList.ForEach(x => x.OnOff(false));
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
