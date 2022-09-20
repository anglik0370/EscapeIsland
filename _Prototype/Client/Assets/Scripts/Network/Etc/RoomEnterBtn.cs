using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomEnterBtn : MonoBehaviour
{
    public Text roomNameText;
    public Text userNumText;
    public Text kidnapperText;
    public int roomNum;

    private Button roomEnterBtn;

    private void Start()
    {
        roomEnterBtn = GetComponent<Button>();
        roomEnterBtn.onClick.AddListener(() =>
        {
            SendManager.Instance.Send("JOIN_ROOM", new RoomVO().SetRoomNum(roomNum));
        });
    }


    public void SetInfo(string roomName, int curUserNum,int userNum, int roomNum,int kidnapperNum)
    {
        roomNameText.text = $"{roomNum}. {roomName}";
        userNumText.text = $"{curUserNum} / {userNum}";
        kidnapperText.text = kidnapperNum.ToString();
        this.roomNum = roomNum;
    }
}
