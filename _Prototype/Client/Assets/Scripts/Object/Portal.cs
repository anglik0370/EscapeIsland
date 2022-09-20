using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private Transform warpPoint;

    private Coroutine co;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.transform.GetComponentInParent<Player>();

        if(!p.IsRemote && p != null)
        {
            SendManager.Instance.Send("NOT_LERP_MOVE", new NotLerpMoveVO(NetworkManager.instance.socketId, warpPoint.position));
        }
    }
}
