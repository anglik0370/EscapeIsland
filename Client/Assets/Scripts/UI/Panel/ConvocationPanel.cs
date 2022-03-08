using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MeetingType
{
    EMERGENCY,
    REPORT,
}

public class ConvocationPanel : Panel
{
    public Text caseTxt;

    protected override void Awake()
    {
        EventManager.SubStartMeet(type =>
        {
            Open(type);
            PopupManager.instance.OpenPopup("vote");
        });

        base.Awake();
    }

    public void Open(MeetingType type)
    {
        switch(type)
        {
            case MeetingType.EMERGENCY:
                caseTxt.text = "��� ȸ��";
                break;
            case MeetingType.REPORT:
                caseTxt.text = "��ü �߰�";
                break;
        }

        base.Open(false);
    }
}
