using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame : Popup
{
    public JoyStick inGameJoyStick;

    private void Start()
    {
        NetworkManager.instance.inGameJoyStick = inGameJoyStick;
    }
}
