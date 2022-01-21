using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    public Transform popupParent;
    private CanvasGroup popupCanvasGroup;

    public Dictionary<string, Popup> popupDic = new Dictionary<string, Popup>();
    private Stack<Popup> popupStack = new Stack<Popup>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("�ټ��� PopupManager�� ������");
            return;
        }
        instance = this;

    }

    //public void OpenPopup(string name, object data = null, int closeCount = 1) // ��ųʸ��� �ִ� UI �������� Ȱ��ȭ
    //{
    //    if (popupStack.Count == 0)
    //    {
    //        DOTween.To(() => popupCanvasGroup.alpha, value => popupCanvasGroup.alpha = value, 1, 0.8f).OnComplete(() =>
    //        {
    //            popupCanvasGroup.interactable = true;
    //            popupCanvasGroup.blocksRaycasts = true;
    //        });
    //    }
    //    popupStack.Push(popupDic[name]);
    //    popupDic[name].Open(data, closeCount);
    //}
    //public void ClosePopup() //UI ��Ȱ��ȭ
    //{
    //    popupStack.Pop().Close();

    //    if (popupStack.Count == 0)
    //    {
    //        DOTween.To(() => popupCanvasGroup.alpha, value => popupCanvasGroup.alpha = value, 0, 0.8f).OnComplete(() =>
    //        {
    //            popupCanvasGroup.interactable = false;
    //            popupCanvasGroup.blocksRaycasts = false;
    //        });
    //    }
    //}
}
