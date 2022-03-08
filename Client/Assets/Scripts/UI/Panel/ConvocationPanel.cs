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

    private const float LIFETIME = 1f;
    private WaitForSeconds ws;

    protected override void Awake()
    {
        EventManager.SubStartMeet(type =>
        {
            Open(type);
            PopupManager.instance.OpenPopup("vote");
        });

        ws = new WaitForSeconds(LIFETIME);

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

        StartCoroutine(DelayClose());

        base.Open(false);
    }

    private IEnumerator DelayClose()
    {
        yield return ws;

        base.Close();
    }
}
