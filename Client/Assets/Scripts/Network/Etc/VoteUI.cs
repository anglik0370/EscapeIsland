using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteUI : MonoBehaviour
{
    public int socId;

    public Button sendVoteBtn;

    public Text nickNameText;
    public Image charImg;
    public Image voteCompleteImg;
    public Image deadImg;

    public Transform userCountParent;

    private void Start()
    {
        sendVoteBtn.onClick.AddListener(SendComplete);
    }

    public void SendComplete()
    {
        VoteCompleteVO vo = new VoteCompleteVO(NetworkManager.instance.socketId, socId);
        DataVO dataVO = new DataVO("VOTE_COMPLETE", JsonUtility.ToJson(vo));
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }

    public void SetVoteUI(int socId, string name, Sprite charSprite, bool isKidnapper)
    {
        this.socId = socId;
        nickNameText.text = name;
        nickNameText.color = isKidnapper && PlayerManager.Instance.AmIKidnapper() ? Color.red : Color.black;
        charImg.sprite = charSprite;

        if (NetworkManager.instance.GetPlayerDie(socId))
        {
            OnDead();
        }

        InitTargeted();
        voteCompleteImg.gameObject.SetActive(false);

        OnOff(true);
    }

    public void OnDead()
    {
        DeadImgActive(true);
    }

    private void DeadImgActive(bool active)
    {
        deadImg.gameObject.SetActive(active);
    }

    public void VoteTargeted()
    {
        UserImg userImg = PoolManager.GetItem<UserImg>();

        userImg.transform.SetParent(userCountParent);
    }

    public void InitTargeted()
    {
        for (int i = 0; i < userCountParent.childCount; i++)
        {
            GameObject userImg = userCountParent.GetChild(i).gameObject;

            if(userImg.activeSelf)
            {
                userImg.SetActive(false);
            }
        }
    }

    public void OnOff(bool on)
    {
        gameObject.SetActive(on);
    }

    public void VoteComplete()
    {
        //checkToggle.gameObject.SetActive(false);
        voteCompleteImg.gameObject.SetActive(true);
    }
}
