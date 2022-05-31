using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VotePopup : Popup
{
    public Button closeChatBtn;
    public Button chatBtn;
    public CanvasGroup chatPanel;

    public Button skipBtn;
    public Transform skipUserParent;

    [Header("CHAT")]
    public List<ChatUI> myChatList = new List<ChatUI>();
    public List<ChatUI> otherChatList = new List<ChatUI>();
    public ScrollRect chatRect;
    public Transform chatParent;
    public GameObject newChatAlert;
    public ChatUI myChat;
    public ChatUI otherChat;

    public InputField msgInputField;
    public Button sendMsgBtn;

    public Transform voteParent;

    public Text middleText;

    public List<VoteUI> voteUIList = new List<VoteUI>();

    public bool IsOpenChatPanel => chatPanel.interactable;

    private void Start()
    {
        voteUIList = voteParent.GetComponentsInChildren<VoteUI>().ToList();
        voteUIList.ForEach(x => x.OnOff(false));

        EventManager.SubGameOver(p =>
        {
            InitChat();
        });

        sendMsgBtn.onClick.AddListener(() =>
        {
            if (msgInputField.text == "") return;

            SendManager.Instance.SendChat(msgInputField.text);
            msgInputField.text = "";
        });
        msgInputField.onEndEdit.AddListener(msg =>
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                sendMsgBtn.onClick?.Invoke();

                msgInputField.text = "";
                msgInputField.ActivateInputField();
            }
        });

        chatBtn.onClick.AddListener(() =>
        {
            CanvasOpenAndClose(chatPanel, true);
            newChatAlert.SetActive(false);
        });

        closeChatBtn.onClick.AddListener(() =>
        {
            CanvasOpenAndClose(chatPanel, false);
        });
    }

    public void ChangeMiddleText(string msg)
    {
        if (VoteManager.Instance.isTextChange) return;
        middleText.text = msg;
    }

    public void CanvasOpenAndClose(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }

    public void SetVoteUI(int socId,string name, Sprite charSprite,bool isKidnapper = false)
    {
        VoteUI ui = voteUIList.Find(x => !x.gameObject.activeSelf);

        if(ui == null)
        {
            Debug.LogError("¾øÀ½");
            return;
        }

        ui.SetVoteUI(socId,name, charSprite,isKidnapper);
    }

    public VoteUI FindVoteUI(int socId)
    {
        return voteUIList.Find(x => x.socId == socId);
    }

    public void CompleteVote()
    {
        //voteUIList.ForEach(x =>
        //{
        //    x.ToggleOnOff(false);
        //});
        //skipToggle.gameObject.SetActive(false);
    }

    public void VoteUIDisable()
    {
        voteUIList.ForEach(x => x.OnOff(false));
    }

    public void VoteBtnDisable()
    {
        VoteEnable(false);
    }

    public void VoteEnable(bool enabled)
    {
        skipBtn.enabled = enabled;
        voteUIList.ForEach(x => x.BtnEnabled(enabled));
    }

    public void AddSkipUser()
    {
        Transform userImg = PoolManager.GetItem<UserImg>().transform;

        userImg.SetParent(skipUserParent);
    }

    public void InitSkipUser()
    {
        for (int i = 0; i < skipUserParent.childCount; i++)
        {
            GameObject userImg = skipUserParent.GetChild(i).gameObject;

            if (userImg.activeSelf)
            {
                userImg.SetActive(false);
            }
        }
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
        StartCoroutine(CoroutineHandler.Frame(() => chatRect.verticalNormalizedPosition = 0f));
    }

    private void InitChat()
    {
        for (int i = 0; i < chatParent.childCount; i++)
        {
            chatParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}
