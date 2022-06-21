using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if ((!p.isDie && !user.isDie) || user.isDie)
                {
                    //voteTab.CreateChat(false, p.socketName, vo.msg, p.curSO.profileImg,p.isDie);
                    //voteTab.newChatAlert.SetActive(!voteTab.IsOpenChatPanel);
                    ChatPanel.Instance.CreateChat(false, p.socketName, vo.msg, p.curSO.profileImg, p.isDie);
                    ChatPanel.Instance.SetChatAlert();
                }
            }
            else
            {
                if (user.socketId == vo.socketId)
                {
                    //voteTab.CreateChat(true, user.socketName, vo.msg, user.curSO.profileImg,user.isDie);
                    ChatPanel.Instance.CreateChat(true, user.socketName, vo.msg, user.curSO.profileImg, user.isDie);
                }
            }
        }
    }
}
