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

        Instance.alertText.color = isEnable ? UtilClass.opacityColor : UtilClass.limpidityColor;
    }

    public void SetUserCountText(int curUser,int maxUser)
    {
        userCountText.text = string.Format(userCountFormat, curUser, maxUser);
    }

    public void AlertText(string msg, AlertType type)
    {
        switch (type)
        {
            case AlertType.Warning:
                alertText.color = Color.red;
                break;
            case AlertType.GameEvent:
                alertText.color = Color.white;
                break;
        }

        alertText.text = msg;

        if (alertSeq != null)
        {
            alertSeq.Kill();
        }

        alertText.color = UtilClass.limpidityColor;

        alertSeq = DOTween.Sequence();

        alertSeq.Append(alertText.DOColor(aleartColorDic[type], 1f));
        alertSeq.Append(alertText.DOColor(UtilClass.limpidityColor, 1f));
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
