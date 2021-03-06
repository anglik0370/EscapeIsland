using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    public Transform popupParent;
    //public ConnectPopup connectPopup;
    public LoginPopup loginPopup;
    public LobbyPopup lobbyPopup;
    public AlertPopup alertPopup;
    
    private CanvasGroup popupCanvasGroup;

    public Dictionary<string, Popup> popupDic = new Dictionary<string, Popup>();
    private Stack<Popup> popupStack = new Stack<Popup>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("?ټ??? PopupManager?? ??????");
            return;
        }
        instance = this;

    }

    private void Start()
    {
        popupCanvasGroup = popupParent.GetComponent<CanvasGroup>();
        if (popupCanvasGroup == null)
        {
            popupCanvasGroup = popupParent.gameObject.AddComponent<CanvasGroup>();
        }
        //?˹??? ?׷? ?ʱ?ȭ
        popupCanvasGroup.alpha = 0;
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;

        LoginPopup loginPopup = Instantiate(this.loginPopup, popupParent);
        LobbyPopup lobbyPopup = Instantiate(this.lobbyPopup, popupParent);

        lobbyPopup.titlePanel = loginPopup.GetComponentInChildren<TitlePanel>();

        //???ųʸ??? ui ?????? ?־??ָ? ??
        //popupDic.Add("connect", Instantiate(connectPopup, popupParent));
        popupDic.Add("login", loginPopup);
        popupDic.Add("lobby", lobbyPopup);

        //alert?? ?׻? ?ؿ? ?־?????
        popupDic.Add("alert", Instantiate(alertPopup, popupParent));
        OpenPopup("login");

        EventManager.SubGameOver(gameOverCase =>
        {
            for(int i = 0; i < popupStack.Count; i++)
            {
                ClosePopup();
            }
        });
    }

    public void CloseAndOpen(string open)
    {
        ClosePopup();
        OpenPopup(open);
    }

    public void OpenPopup(string name, object data = null, int closeCount = 1) // ???ųʸ??? ?ִ? UI ???????? Ȱ??ȭ
    {
        if (popupStack.Count == 0)
        {
            DOTween.To(() => popupCanvasGroup.alpha, value => popupCanvasGroup.alpha = value, 1, 0.8f).OnComplete(() =>
            {
                popupCanvasGroup.interactable = true;
                popupCanvasGroup.blocksRaycasts = true;
            });
        }
        popupStack.Push(popupDic[name]);
        popupDic[name].Open(data, closeCount);
    }
    public void ClosePopup() //UI ??Ȱ??ȭ
    {
        if (popupStack.Count == 0) return;

        popupStack.Pop().Close();

        if (popupStack.Count == 0)
        {
            DOTween.To(() => popupCanvasGroup.alpha, value => popupCanvasGroup.alpha = value, 0, 0.8f).OnComplete(() =>
            {
                popupCanvasGroup.interactable = false;
                popupCanvasGroup.blocksRaycasts = false;
            });
        }
    }
}
