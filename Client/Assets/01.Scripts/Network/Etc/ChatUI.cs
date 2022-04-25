using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public Text nameText;
    public Text msgText;
    public Image charImg;


    public void SetChatUI(string name, string msg, Sprite charSpr,Transform parent)
    {
        nameText.text = name;
        msgText.text = msg;
        charImg.sprite = charSpr;

        transform.SetParent(parent);
        gameObject.SetActive(true);
    }
}
