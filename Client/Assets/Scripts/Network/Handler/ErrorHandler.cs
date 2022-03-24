using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //print(payload);
        UIManager.Instance.SetWarningText(payload);
    }
}
