using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineHandler 
{
    public static WaitForSeconds zeroFourSec = new WaitForSeconds(0.4f);
    public static WaitForSeconds oneSec = new WaitForSeconds(1f);
    public static WaitForSeconds fifteenSec = new WaitForSeconds(15f);

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
}
