using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineryPanel : Panel
{
    public Image oreSlotImg;
    public Image ingotSlotImg;

    public Text oreNameText;
    public Text ingotNameText;

    public Image progressArrowImg;
    public Text progressTimeText;

    public Refinery nowOpenRefinery;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Open(Refinery refinery)
    {
        base.Open();

        nowOpenRefinery = refinery;
    }

    public override void Close()
    {
        base.Close();

        nowOpenRefinery = null;
    }
}
