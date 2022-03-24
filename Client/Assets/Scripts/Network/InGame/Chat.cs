using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : ISetAble
{
    private Queue<ChatVO> chatQueue = new Queue<ChatVO>();
    private VotePopup voteTab = null;

    public void ReceiveChat(ChatVO vo)
    {
        lock (lockObj)
        {
            chatQueue.Enqueue(vo);
        }
    }

    protected override void Start()
    {
        StartCoroutine(End());
    }

    void Update()
    {
        while (chatQueue.Count > 0)
        {
            Init();
            ChatVO vo = chatQueue.Dequeue();
            print("ChatHandler");

            Player p = null;

            if (playerList.TryGetValue(vo.socketId, out p))
            {
                if ((!p.isDie && !user.isDie) || user.isDie)
                {
                    voteTab.CreateChat(false, p.socketName, vo.msg, p.curSO.profileImg);
                }
            }
            else
            {
                if (user.socketId == vo.socketId)
                {
                    voteTab.CreateChat(true, user.socketName, vo.msg, user.curSO.profileImg);
                }
            }
        }
    }

    IEnumerator End()
    {
        yield return null;
        voteTab = VoteManager.Instance.voteTab;
    }
}
