using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineryPanel : Panel
{
    public static RefineryPanel Instance;

    public ItemSlot oreSlot;
    public ItemSlot ingotSlot;

    public Refinery nowOpenRefinery;

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();
    }

    private void Update() 
    {
        
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
