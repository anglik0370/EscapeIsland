using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineHandler 
{

    public static IEnumerator Frame(Action act)
    {
        yield return null;
        act?.Invoke();
    }
}
