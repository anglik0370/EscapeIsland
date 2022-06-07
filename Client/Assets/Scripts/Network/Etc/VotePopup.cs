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

    [SerializeField]
    private Text timeInfoText;

    [Header("CHAT")]
    private List<ChatUI> myChatList = new List<ChatUI>();
    private List<ChatUI> otherChatList = new List<ChatUI>();
    [SerializeField]
    private Transform myChatParent;
    [SerializeField]
    private Transform otherChatParent;

    [SerializeField]
    private ContentSizeFitter chatSizeFitter;

    private const string NEW_LINE = "\n";
    private ChatUI lastChatUI;

    public ScrollRect chatRect;
    public Transform chatParent;
    public GameObject newChatAlert;
    public ChatUI myChat;
    public ChatUI otherChat;

    public InputField msgInputField;
    public Button sendMsgBtn;

    public Transform voteParent;
    public VoteTimeBar voteTimeBar;

    //public Text middleText;

    public List<VoteUI> voteUIList = new List<VoteUI>();

    public bool IsOpenChatPanel => chatPanel.interactable;

    private void Start()
    {
        voteUIList = voteParent.GetComponentsInChildren<VoteUI>().ToList();
        myChatList = myChatParent.GetComponentsInChildren<ChatUI>().ToList();
        otherChatList = otherChatParent.GetComponentsInChildren<ChatUI>().ToList();

        voteUIList.ForEach(x => x.OnOff(false));

        EventManager.SubGameOver(p =>
        {
            InitChat();
            InitSkipUser();
        });
        EventManager.SubStartMeet(mt => Init());

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
            chatRect.verticalNormalizedPosition = 0f;
            //StartCoroutine(CoroutineHandler.Frame(() => chatRect.verticalNormalizedPosition = 0f));
        });

        closeChatBtn.onClick.AddListener(() =>
        {
            CanvasOpenAndClose(chatPanel, false);
        });
    }

    //public void ChangeMiddleText(string msg)
    //{
    //    if (VoteManager.Instance.isTextChange) return;
    //    middleText.text = msg;
    //}

    public void CanvasOpenAndClose(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }

    public void SetTimeInfoText(string msg)
    {
        timeInfoText.text = msg;
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

        userImg.localScale = Vector3.one;
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

    public void CreateChat(bool myChat,string name, string chatMsg, Sprite charSpr,bool isDie)
    {
        if(lastChatUI != null)
        {
            bool isSameUser = name == lastChatUI.nameText.text;

            if (isSameUser)
            {
                chatMsg = NEW_LINE + chatMsg;
                lastChatUI.SetSameUserText(chatMsg);
                LayoutRebuilder.MarkLayoutForRebuild(chatParent as RectTransform);
                StartCoroutine(CoroutineHandler.EndFrame(ImmediateLayout));
                return;
            }
        }

       

        ChatUI ui = null;

        if(myChat)
        {
            ui = myChatList.Find(x => !x.gameObject.activeSelf);

            if(ui == null)
            {
                ui = Instantiate(this.myChat, transform);
                myChatList.Add(ui);
            }

            ui.SetChatUI(name, chatMsg, charSpr,chatParent,isDie);
        }
        else
        {
            ui = otherChatList.Find(x => !x.gameObject.activeSelf);

            if(ui == null)
            {
                ui = Instantiate(otherChat, transform);
                otherChatList.Add(ui);
            }

            ui.SetChatUI(name, chatMsg, charSpr,chatParent,isDie);
        }

        lastChatUI = ui;

        StartCoroutine(CoroutineHandler.EndFrame(ImmediateLayout));
    }

    private void InitChat()
    {
        for (int i = 0; i < chatParent.childCount; i++)
        {
            chatParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Init()
    {
        lastChatUI = null;
    }

    private void ImmediateLayout()
    {
        //LayoutRebuilder.MarkLayoutForRebuild(chatParent as RectTransform);

        chatRect.verticalNormalizedPosition = 0f;

        //chatSizeFitter.enabled = false;
        //chatSizeFitter.enabled = true;
    }
}
