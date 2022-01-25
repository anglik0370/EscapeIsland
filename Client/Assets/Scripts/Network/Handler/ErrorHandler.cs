using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        PopupManager.instance.OpenPopup("alert", payload);
    }
}
