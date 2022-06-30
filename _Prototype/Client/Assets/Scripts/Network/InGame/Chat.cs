using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChatType
{
    None = 0,
    Team,
    All,
}

public class Chat : ISetAble
{
    private Queue<ChatVO> chatQueue = new Queue<ChatVO>();

    public void ReceiveChat(ChatVO vo)
    {
        lock (lockObj)
        {
            chatQueue.Enqueue(vo);
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        while (chatQueue.Count > 0)
        {
            Init();
            ChatVO vo = chatQueue.Dequeue();

            Player p = null;

            if (playerList.TryGetValue(vo.socketId, out p))
            {
                ChatPanel.Instance.CreateChat(false, p.socketName, vo.msg, p.curSO.profileImg);
                ChatPanel.Instance.SetChatAlert();
            }
            else
            {
                ChatPanel.Instance.CreateChat(true, user.socketName, vo.msg, user.curSO.profileImg);

            }
        }
    }
}
