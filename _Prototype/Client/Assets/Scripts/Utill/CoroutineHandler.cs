using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineHandler 
{
    public static WaitForSeconds zeroEightSec = new WaitForSeconds(0.8f);
    public static WaitForSeconds oneSec = new WaitForSeconds(1f);
    public static WaitForSeconds tenSec = new WaitForSeconds(10f);

    public static IEnumerator Frame(Action act)
    {
        yield return null;
        act?.Invoke();
    }

    public static IEnumerator EndFrame(Action act)
    {
        yield return new WaitForEndOfFrame();
        act?.Invoke();
    }

    public static IEnumerator EnableDampingEndFrame(CinemachineVirtualCamera vcam)
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
