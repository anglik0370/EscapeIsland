using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertPopup : Popup
{
    public Text alertText;
    public Button confirmBtn;

    private void Start()
    {
        confirmBtn.onClick.AddListener(() =>
        {
            PopupManager.instance.ClosePopup();
        });
    }

    public override void Open(object data = null, int closeCount = 1)
    {
        base.Open(data, closeCount);

        alertText.text = (string)data;
    }
}
