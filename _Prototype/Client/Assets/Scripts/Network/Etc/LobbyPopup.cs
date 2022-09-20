using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopup : Popup
{
    public TitlePanel titlePanel;

    public Button refreshBtn;
    public Button createPopupOpenBtn;
    public Button joinPopupOpenBtn;
    public Button exitBtn;

    public Transform roomParent;

    [Header("CreateRoomPopup")]
    public CanvasGroup createRoomPopup;
    public InputField roomNameInput;
    public Button createRoomBtn;
    public Button cancelBtn;

    [Header("JoinRoom")]
    public CanvasGroup joinRoomPopup;
    public InputField joinRoomNameInput;
    public Button joinBtn;
    public Button joinCancelBtn;

    public Slider userNumslider;
    public Text userNumtext;

    public Toggle testToggle;

    private bool isOpenPanel = false;

    private void Start()
    {
        UIManager.Instance.OnEndEdit(roomNameInput, createRoomBtn.onClick);
        UIManager.Instance.OnEndEdit(joinRoomNameInput, joinBtn.onClick);

        refreshBtn.onClick.AddListener(() =>
        {
            SendManager.Instance.Send("ROOM_REFRESH_REQ");
        });
        createPopupOpenBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(true);
        });
        exitBtn.onClick.AddListener(() =>
        {
            //NetworkManager.instance.ExitRoom();
            PopupManager.instance.CloseAndOpen("login");
            titlePanel.Init();
        });

        createRoomBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.FindSetDataScript<RefreshUsers>().isTest = testToggle.isOn;
            SendManager.Instance.Send("CREATE_ROOM", new RoomVO(roomNameInput.text, 0, 0, (int)userNumslider.value, 0));
            OpenCreateRoomPopup(false);
        });
        cancelBtn.onClick.AddListener(() =>
        {
            OpenCreateRoomPopup(false);
        });

        joinPopupOpenBtn.onClick.AddListener(() =>
        {
            OpenJoinRoomPopup(true);
        });

        joinCancelBtn.onClick.AddListener(() => OpenJoinRoomPopup(false));

        joinBtn.onClick.AddListener(() =>
        {
            //JoinRoom 보내 nameInput text로 
            SendManager.Instance.Send("FIND_ROOM", new RoomVO().SetRoomName(joinRoomNameInput.text));
            OpenJoinRoomPopup(false);
        });


        userNumslider.onValueChanged.AddListener(x =>
        {
            userNumtext.text = $"{(int)x}";
        });
    }

    private void Update()
    {
        if (!isOpenPanel) return;

        if(Input.GetKeyDown(KeyCode.C))
        {
            SendManager.Instance.Send("ROOM_DELETE_REQ");
        }
    }

    public override void Open(object data = null, int closeCount = 1)
    {
        isOpenPanel = true;
        base.Open(data, closeCount);
    }

    public override void Close()
    {
        isOpenPanel = false;
        base.Close();
    }

    public void OpenCreateRoomPopup(bool on)
    {
        CgOpenAndClose(createRoomPopup, on);

        if(!on)
        {
            roomNameInput.text = "";
            userNumslider.value = 5;
            testToggle.isOn = false;
        }
        else
        {
            roomNameInput.ActivateInputField();
        }
    }

    public void OpenJoinRoomPopup(bool on)
    {
        CgOpenAndClose(joinRoomPopup, on);

        if(!on)
        {
            joinRoomNameInput.text = "";
        }
        else
        {
            joinRoomNameInput.ActivateInputField();
        }
    }

    public void CgOpenAndClose(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }
}
