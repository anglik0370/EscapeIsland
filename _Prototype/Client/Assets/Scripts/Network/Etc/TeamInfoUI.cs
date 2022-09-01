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
    private Image readyImg;
    [SerializeField]
    private Image hostImg;
    [SerializeField]
    private Image backgroundImg;

    [Header("준비 상태 스프라이트")]
    [SerializeField]
    private Sprite unreadySprite;
    [SerializeField]
    private Sprite readySprite;

    [Header("팀 상태 스프라이트")]
    [SerializeField]
    private Sprite redbackgroundSprite;
    [SerializeField]
    private Sprite bluebackgroundSprite;

    private Player player;
    public int SocketId => player.socketId;

    private void Start()
    {
        EventManager.SubBackToRoom(() =>
        {
            if (player != null && player.master) return;

            SetReadyImg(false);
        });
        EventManager.SubExitRoom(() =>
        {
            if (player != null && player.master) return;

            SetReadyImg(false);
        });
    }

    public void SetUser(string name, Player p)
    {
        nameText.text = name;

        player = p;

        if(player.master)
        {
            SetReadyImg(true, true);
        }

        RefreshProfile();
    }

    public void SetParent(Transform parent)
    {
        RefreshProfile();

        if (player.CurTeam == Team.RED)
        {
            backgroundImg.sprite = redbackgroundSprite;
        }
        else if (player.CurTeam == Team.BLUE)
        {
            backgroundImg.sprite = bluebackgroundSprite;
        }

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

    public void SetReadyImg(bool isReady, bool isHost = false)
    {
        if(isHost)
        {
            readyImg.color = UtilClass.limpidityColor;
            hostImg.color = UtilClass.opacityColor;
        }
        else
        {
            readyImg.color = UtilClass.opacityColor;
            hostImg.color = UtilClass.limpidityColor;

            if (isReady)
            {
                readyImg.sprite = readySprite;
            }
            else
            {
                readyImg.sprite = unreadySprite;
            }
        }
    }
}
