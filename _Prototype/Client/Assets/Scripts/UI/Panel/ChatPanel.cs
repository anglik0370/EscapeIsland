using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : Panel
{
    public static ChatPanel Instance { get; private set; }

    [SerializeField]
    private Button chatOpenBtn;
    [SerializeField]
    private Button sendChatBtn;

    [SerializeField]
    private InputField chatInput;

    [SerializeField]
    private Transform myChatParent;
    [SerializeField]
    private Transform otherChatParent;
    [SerializeField]
    private Transform chatParent;

    [SerializeField]
    private GameObject newChatAlertObj;

    [SerializeField]
    private ScrollRect chatRect;

    private ChatUI lastChatUI;

    [SerializeField]
    private ChatUI myChat;
    [SerializeField]
    private ChatUI otherChat;

    private Queue<ChatUI> myChatQueue = new Queue<ChatUI>();
    private Queue<ChatUI> otherChatQueue = new Queue<ChatUI>();

    [SerializeField]
    private Button switchingBtn;
    [SerializeField]
    private CanvasGroup cvsChat;
    [SerializeField]
    private CanvasGroup cvsLog;

    private Text chatStateTxt;
    private const string LOG_STRING = "로그";
    private const string CHAT_STRING = "채팅";

    private const string NEW_LINE = "\n";

    private ChatType chatType = ChatType.None;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;

        chatStateTxt = switchingBtn.GetComponentInChildren<Text>();
    }

    protected override void Start()
    {
        base.Start();

        EventManager.SubEnterRoom(p =>
        {
            InitChat();
            chatType = ChatType.All;
        });

        EventManager.SubBackToRoom(() =>
        {
            InitChat();
            chatType = ChatType.All;
        });

        EventManager.SubGameStart(p =>
        {
            InitChat();
            chatType = ChatType.Team;
        });

        sendChatBtn.onClick.AddListener(() =>
        {
            if (chatInput.text == "") return;

            SendManager.Instance.Send("CHAT", new ChatVO(NetworkManager.instance.socketId, chatInput.text, chatType));
            chatInput.text = "";
            SoundManager.Instance.PlayBtnSfx();
        });

        chatInput.onEndEdit.AddListener(msg =>
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sendChatBtn.onClick?.Invoke();

                chatInput.text = "";
                chatInput.ActivateInputField();
            }
        });

        chatOpenBtn.onClick.AddListener(() =>
        {
            if (cvs.interactable)
            {
                Close();
                return;
            }

            Open();
            SoundManager.Instance.PlayBtnSfx();
            newChatAlertObj.SetActive(false);
            chatRect.verticalNormalizedPosition = 0f;
            //StartCoroutine(CoroutineHandler.Frame(() => chatRect.verticalNormalizedPosition = 0f));
        });

        chatStateTxt.text = CHAT_STRING;

        switchingBtn.onClick.AddListener(() =>
        {
            if (chatStateTxt.text == CHAT_STRING)
            {
                chatStateTxt.text = LOG_STRING;

                UtilClass.SetCanvasGroup(cvsChat);
                UtilClass.SetCanvasGroup(cvsLog, 1f, true, true);
            }
            else
            {
                chatStateTxt.text = CHAT_STRING;

                UtilClass.SetCanvasGroup(cvsLog);
                UtilClass.SetCanvasGroup(cvsChat, 1f, true, true);
            }
            SoundManager.Instance.PlayBtnSfx();
        });

        // chat ui 
        {
            for (int i = 0; i < myChatParent.childCount; i++)
            {
                myChatQueue.Enqueue(myChatParent.GetChild(i).GetComponent<ChatUI>());
            }

            for (int i = 0; i < otherChatParent.childCount; i++)
            {
                otherChatQueue.Enqueue(otherChatParent.GetChild(i).GetComponent<ChatUI>());
            }
        }
    }

    public void CreateChat(bool myChat, string name, string chatMsg, Sprite charSpr)
    {
        if (lastChatUI != null)
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

        if (myChat)
        {
            ui = myChatQueue.Count > 0 ? myChatQueue.Dequeue() : Instantiate(this.myChat, myChatParent);
        }
        else
        {
            ui = otherChatQueue.Count > 0 ? otherChatQueue.Dequeue() : Instantiate(otherChat, otherChatParent);
        }

        ui.SetChatUI(name, chatMsg, charSpr, chatParent);
        lastChatUI = ui;

        StartCoroutine(CoroutineHandler.EndFrame(ImmediateLayout));
    }

    private void InitChat()
    {
        for (int i = 0; i < chatParent.childCount; i++)
        {
            Transform t = chatParent.GetChild(i);
            ChatUI chat = t.GetComponent<ChatUI>();

            t.gameObject.SetActive(false);

            if (chat.nameText.text.Equals(NetworkManager.instance.socketName))
            {
                myChatQueue.Enqueue(chat);
            }
            else
            {
                otherChatQueue.Enqueue(chat);
            }
        }

        chatStateTxt.text = CHAT_STRING;

        UtilClass.SetCanvasGroup(cvsLog);
        UtilClass.SetCanvasGroup(cvsChat, 1f, true, true);

        Init();
    }

    public void SetChatAlert()
    {
        newChatAlertObj.SetActive(!cvs.interactable);
    }

    private void Init()
    {
        lastChatUI = null;
        newChatAlertObj.SetActive(false);
        Close();
    }

    private void ImmediateLayout()
    {
        //LayoutRebuilder.MarkLayoutForRebuild(chatParent as RectTransform);

        chatRect.verticalNormalizedPosition = 0f;

        //chatSizeFitter.enabled = false;
        //chatSizeFitter.enabled = true;
    }
}
