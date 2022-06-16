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

    public void SetUser(string name, Player p)
    {
        nameText.text = name;

        player = p;

        RefreshProfile();
    }

    public void SetParent(Transform parent)
    {
        RefreshProfile();

        transform.SetParent(parent);
    }

    private void RefreshProfile()
    {
        if (player.curSO != null)
            profileImg.sprite = player.curSO.profileImg;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
