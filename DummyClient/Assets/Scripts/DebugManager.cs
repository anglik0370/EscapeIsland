using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public enum SendType
{
    LOGIN = 0,
    CREATE_ROOM,
    JOIN_ROOM,
    KILL,
    GAME_START,
    STORAGE_FULL,
    EMERGENCY,
    VOTE,
    ISINSIDE,
    WIN,
    SABOTAGE,
}

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }

    private int socketId;
    private int roomNum;
    private string socketName;

    private SendType curSendType = SendType.LOGIN;
    public SendType CurSendType => curSendType;

    private Dictionary<SendType, CanvasGroup> panelDic;

    public Transform cgParent;
    private CanvasGroup[] cgArr;
    [SerializeField]
    private Dropdown sendTypeDropdown;
    [SerializeField]
    private Button sendBtn;

    [Header("로그인 관련 UI들")]
    public InputField nameInputField;

    [Header("룸 생성 관련 UI들")]
    public InputField roomNameInputField;
    public InputField kidnapperCountInputField;
    public InputField maxPlayerInputField;

    [Header("룸 조인 관련 UI들")]
    public InputField joinRoomNameInputField;

    [Header("킬 관련 UI들")]
    public InputField targetSocketIdInputField;

    [Header("투표 관련 UI들")]
    public InputField voteTargetSocketIdInputField;

    [Header("커버 관련 UI들")]
    public Toggle isInsideToggle;

    [Header("Win 관련 UI들")]
    public InputField gocInputField;

    [Header("사보타지 관련 UI들")]
    public InputField sabotageNameInputField;
    public Toggle isShareCoolTimeToggle;

    private void Awake()
    {
        Instance = this;

        panelDic = new Dictionary<SendType, CanvasGroup>();
    }

    public void InitData(int socketId, int roomNum, string name)
    {
        this.socketId = socketId;
        this.roomNum = roomNum;
        this.socketName = name;
    }

    public void ChangeRoom(int roomNum)
    {
        this.roomNum = roomNum;
    }

    private void Start()
    {
        sendTypeDropdown.onValueChanged.AddListener(x =>
        {
            curSendType = (SendType)x;
            ChangeSendType(CurSendType);
        });

        cgArr = cgParent.GetComponentsInChildren<CanvasGroup>();

        for (int i = 0; i < cgArr.Length; i++)
        {
            panelDic.Add((SendType)i, cgArr[i]);
        }

        curSendType = SendType.LOGIN;
        ChangeSendType(curSendType);
    }

    private void ChangeSendType(SendType type)
    {
        foreach (SendType st in panelDic.Keys)
        {
            CanvasGroupOnOff(panelDic[st], st == type);
        }
        SetSendBtn(type);
    }

    private void CanvasGroupOnOff(CanvasGroup cg, bool on)
    {
        cg.alpha = on ? 1f : 0f;
        cg.interactable = on;
        cg.blocksRaycasts = on;
    }

    private void SetSendBtn(SendType type)
    {
        sendBtn.onClick.RemoveAllListeners();
        sendBtn.onClick.AddListener(() => SendMessage(type.ToString()));
    }

    private void Send(string type)
    {
        DataVO dataVO = new DataVO(type, null);
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    private void EMERGENCY()
    {
        Send("EMERGENCY");
    }

    private void LOGIN()
    {
        LoginVO vo = new LoginVO(nameInputField.text);

        DataVO dataVO = new DataVO("LOGIN", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        nameInputField.text = "";
    }

    private void CREATE_ROOM()
    {
        RoomVO vo = new RoomVO(roomNameInputField.text, 0, 0, int.Parse(maxPlayerInputField.text), int.Parse(kidnapperCountInputField.text));

        DataVO dataVO = new DataVO("CREATE_ROOM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        roomNameInputField.text = "";
        maxPlayerInputField.text = "";
        kidnapperCountInputField.text = "";
    }

    private void JOIN_ROOM()
    {
        RoomVO vo = new RoomVO();
        vo.name = joinRoomNameInputField.text;

        DataVO dataVO = new DataVO("FIND_ROOM", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        joinRoomNameInputField.text = "";
    }

    private void KILL()
    {
        KillVO vo = new KillVO(int.Parse(targetSocketIdInputField.text));

        DataVO dataVO = new DataVO("KILL", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        targetSocketIdInputField.text = "";
    }

    private void GAME_START()
    {
        RoomVO vo = new RoomVO();
        vo.roomNum = roomNum;

        DataVO dataVO = new DataVO("GameStart", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    private void STORAGE_FULL()
    {
        DataVO dataVO = new DataVO("STORAGE_FULL", "");

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    private void VOTE()
    {
        VoteCompleteVO vo = new VoteCompleteVO(socketId, voteTargetSocketIdInputField.text == "" ? -1 : int.Parse(voteTargetSocketIdInputField.text));

        DataVO dataVO = new DataVO("VOTE_COMPLETE", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    private void ISINSIDE()
    {
        UserVO vo = new UserVO();
        vo.isInside = isInsideToggle.isOn;

        DataVO dataVO = new DataVO("INSIDE_REFRESH", JsonUtility.ToJson(vo));
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    private void WIN()
    {
        WinVO vo = new WinVO(int.Parse(gocInputField.text));

        DataVO dataVO = new DataVO("WIN", JsonUtility.ToJson(vo));
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    private void SABOTAGE()
    {
        SabotageVO vo = new SabotageVO(isShareCoolTimeToggle.isOn, sabotageNameInputField.text);

        DataVO dataVO = new DataVO("SABOTAGE", JsonUtility.ToJson(vo));

        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
