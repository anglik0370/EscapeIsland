using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : Panel
{
    void Start()
    {
        EventManager.SubExitRoom(() =>
        {
            Close();
        });
    }
}
