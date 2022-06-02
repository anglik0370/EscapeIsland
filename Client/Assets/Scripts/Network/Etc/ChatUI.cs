using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public Text nameText;
    public Text msgText;
    public Image charImg;
    public CanvasGroup cg;

    public void SetChatUI(string name, string msg, Sprite charSpr,Transform parent,bool isDie)
    {
        nameText.text = name;
        msgText.text = msg;
        charImg.sprite = charSpr;

        transform.SetParent(parent);

        cg.alpha = isDie ? 0.5f : 1f;
        gameObject.SetActive(true);
    }
}
