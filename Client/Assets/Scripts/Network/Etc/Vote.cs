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
    public Toggle skipToggle;

    public List<VoteUI> voteUIList = new List<VoteUI>();

    private void Start()
    {
        voteUIList = voteParent.GetComponentsInChildren<VoteUI>().ToList();
        voteUIList.ForEach(x => x.OnOff(false));

        //skipToggle.group = toggleGroup;

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
            Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();

            if(toggle == null)
            {
                print("투표 x");
                return;
            }

            VoteUI ui = toggle.GetComponentInParent<VoteUI>();

            int selectSocket = ui == null ? -1 : ui.socId;
            VoteCompleteVO vo = new VoteCompleteVO(NetworkManager.instance.socketId, selectSocket);

            DataVO dataVO = new DataVO("VOTE_COMPLETE", JsonUtility.ToJson(vo));

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        });

    }

    public void CanvasOpenAndClose(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }

    public void SetVoteUI(int socId,string name, Sprite charSprite)
    {
        VoteUI ui = voteUIList.Find(x => !x.gameObject.activeSelf);

        if(ui == null)
        {
            Debug.LogError("없음");
            return;
        }

        ui.SetVoteUI(socId,name, charSprite, toggleGroup);
    }

    public VoteUI FindVoteUI(int socId)
    {
        return voteUIList.Find(x => x.socId == socId);
    }

    public void CompleteVote()
    {
        voteUIList.ForEach(x =>
        {
            x.ToggleOnOff(false);
        });
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
