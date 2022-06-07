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
            if(co != null)
            {
                StopCoroutine(co);
            }

            co = StartCoroutine(EnableDampingEndFrame(GameManager.Instance.CmVCam));

            p.transform.position = warpPoint.position;
        }
    }

    private IEnumerator EnableDampingEndFrame(CinemachineVirtualCamera vcam)
    {
        float xDamping = vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        float yDamping = vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping;
        float zDamping = vcam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping;

        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0f;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0f;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0f;

        yield return null;

        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = xDamping;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = yDamping;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = zDamping;
    }
}
