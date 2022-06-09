using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotLerpMoveHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        NotLerpMoveVO vo = JsonUtility.FromJson<NotLerpMoveVO>(payload);

        RefreshUsers.SetNotLerpMovedata(vo);
    }
}
