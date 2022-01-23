using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomEnterBtn : MonoBehaviour
{
    public Text roomNameText;
    public Text userNumText;
    public int roomNum;

    private Button roomEnterBtn;

    private void Start()
    {
        roomEnterBtn = GetComponent<Button>();
        roomEnterBtn.onClick.AddListener(() =>
        {
            NetworkManager.instance.JoinRoom(roomNum);
        });
    }


    public void SetInfo(string roomName, int curUserNum,int userNum, int roomNum)
    {
        roomNameText.text = roomName;
        userNumText.text = $"{curUserNum} / {userNum}";
        this.roomNum = roomNum;
    }
}
