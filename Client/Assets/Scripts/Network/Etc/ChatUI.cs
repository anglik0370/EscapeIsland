using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public Text nameText;
    public Text msgText;
    public RectTransform msgTextRect;
    public RectTransform fieldRect;
    public Image charImg;
    public CanvasGroup cg;

    public RectTransform chatRect;
    public RectTransform topRect;
    //public RectTransform botRect;

    public void SetChatUI(string name, string msg, Sprite charSpr,Transform parent,bool isDie)
    {
        nameText.text = name;
        msgText.text = msg;
        charImg.sprite = charSpr;

        transform.SetParent(parent);

        cg.alpha = isDie ? 0.5f : 1f;
        gameObject.SetActive(true);
    }

    public void SetSameUserText(string msg)
    {
        msgText.text += msg;

        //Canvas.ForceUpdateCanvases();
        fieldRect.sizeDelta = new Vector2(fieldRect.sizeDelta.x, msgTextRect.rect.height);
        chatRect.sizeDelta = new Vector2(chatRect.sizeDelta.x, topRect.rect.height + fieldRect.rect.height);
    }
}
