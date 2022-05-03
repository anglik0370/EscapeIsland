using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteUI : MonoBehaviour
{
    public int socId;

    public Text nickNameText;
    public Image charImg;
    public Toggle checkToggle;
    public Image voteCompleteImg;

    public void SetVoteUI(int socId, string name, Sprite charSprite, ToggleGroup group,bool isKidnapper)
    {
        this.socId = socId;
        nickNameText.text = name;
        nickNameText.color = isKidnapper && NetworkManager.instance.User.isKidnapper ? Color.red : Color.black;
        charImg.sprite = charSprite;
        checkToggle.group = group;

        checkToggle.isOn = false;
        if(NetworkManager.instance.User.isDie || NetworkManager.instance.GetPlayerDie(socId))
        {
            ToggleOnOff(false);

        }
        else
        {
            ToggleOnOff(true);
        }

        voteCompleteImg.gameObject.SetActive(false);

        OnOff(true);
    }

    public void OnOff(bool on)
    {
        gameObject.SetActive(on);
    }

    public void ToggleOnOff(bool on)
    {
        checkToggle.gameObject.SetActive(on);
    }

    public void VoteComplete()
    {
        //checkToggle.gameObject.SetActive(false);
        voteCompleteImg.gameObject.SetActive(true);
    }
}
