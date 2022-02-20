using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteUI : MonoBehaviour
{
    public Text nickNameText;
    public Image charImg;
    public Toggle checkToggle;
    public Image voteCompleteImg;

    public void SetVoteUI(string name,Sprite charSprite,ToggleGroup group)
    {
        nickNameText.text = name;
        charImg.sprite = charSprite;
        checkToggle.group = group;

        checkToggle.isOn = false;
        checkToggle.gameObject.SetActive(true);
        voteCompleteImg.gameObject.SetActive(false);

        OnOff(true);
    }

    public void OnOff(bool on)
    {
        gameObject.SetActive(on);
    }

    public void VoteComplete()
    {
        checkToggle.gameObject.SetActive(false);
        voteCompleteImg.gameObject.SetActive(true);
    }
}
