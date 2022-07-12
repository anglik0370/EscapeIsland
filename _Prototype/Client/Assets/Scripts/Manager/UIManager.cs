using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public enum AlertType
{
    Warning,
    GameEvent,
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public CanvasGroup panels;

    [SerializeField]
    private CanvasGroup cvsAlert;
    [SerializeField]
    private Text alertText;
    [SerializeField]
    private Text userCountText;

    private string userCountFormat = "{0}/{1}";

    private Sequence alertSeq;

    private Dictionary<AlertType, Color> aleartColorDic = new Dictionary<AlertType, Color>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        SetPanelActive(false);

        aleartColorDic.Add(AlertType.GameEvent, Color.white);
        aleartColorDic.Add(AlertType.Warning, Color.red);
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            SetPanelActive(true);
            AlertText("방에 들어왔습니다.", AlertType.GameEvent);
        });

        EventManager.SubGameStart(p =>
        {
            AlertText("게임이 시작되었습니다.", AlertType.GameEvent);
        });

        EventManager.SubExitRoom(() =>
        {
            SetPanelActive(false);
        });
    }

    public static void SetPanelActive(bool isEnable)
    {
        Instance.panels.alpha = isEnable ? 1f : 0f;
        Instance.panels.interactable = isEnable;
        Instance.panels.blocksRaycasts = isEnable;

        Instance.cvsAlert.alpha = isEnable ? 1f : 0f;
    }

    public void SetUserCountText(int curUser,int maxUser)
    {
        userCountText.text = string.Format(userCountFormat, curUser, maxUser);
    }

    public void AlertText(string msg, AlertType type)
    {
        alertText.text = msg;
        alertText.color = aleartColorDic[type];

        if (alertSeq != null)
        {
            alertSeq.Kill();
        }

        cvsAlert.alpha = 0;

        alertSeq = DOTween.Sequence();

        alertSeq.Append(DOTween.To(() => cvsAlert.alpha, x => cvsAlert.alpha = x, 1f, 1.5f));
        alertSeq.Append(DOTween.To(() => cvsAlert.alpha, x => cvsAlert.alpha = x, 0f, 1.5f));
    }

    public void OnEndEdit(InputField inputField, Button.ButtonClickedEvent onClickEvent)
    {
        inputField.onEndEdit.AddListener(msg =>
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                onClickEvent?.Invoke();

                inputField.text = "";

            }
        });
    }
}
