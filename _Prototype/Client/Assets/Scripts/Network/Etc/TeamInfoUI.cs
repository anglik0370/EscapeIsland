using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamInfoUI : MonoBehaviour
{
    [SerializeField]
    private Image profileImg;

    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text readyText;

    private Player player;
    public int SocketId => player.socketId;

    private void Start()
    {
        EventManager.SubBackToRoom(() =>
        {
            if (player != null && player.master) return;

            SetReadyText(false);
        });
        EventManager.SubExitRoom(() =>
        {
            if (player != null && player.master) return;

            SetReadyText(false);
        });
    }

    public void SetUser(string name, Player p)
    {
        nameText.text = name;

        player = p;

        if(player.master)
        {
            SetReadyText(true, UtilClass.HOST_TEXT);
        }

        RefreshProfile();
    }

    public void SetParent(Transform parent)
    {
        RefreshProfile();

        transform.SetParent(parent);
    }

    public void RefreshProfile()
    {
        if (player.curSO != null)
            profileImg.sprite = player.curSO.profileImg;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetReadyText(bool on,string msg = null)
    {
        if(msg != null)
        {
            readyText.text = msg;
        }
        readyText.enabled = on;
    }
}
