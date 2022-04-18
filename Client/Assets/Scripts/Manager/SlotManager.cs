using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SlotManager : MonoBehaviour
{
    public static SlotManager Instance { get; private set; }

    [SerializeField]
    private ItemSlot beginSlot; //드래그 시작
    [SerializeField]
    private ItemSlot endSlot;   //드래그 끝

    [SerializeField]
    private ItemGhost ghost;

    [SerializeField]
    private bool isDraging = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if(isDraging)
        {
            ghost.SetPosition(Input.mousePosition);
        }
    }

    public void BeginDrag(ItemSlot slot)
    {
        beginSlot = slot;

        ghost.SetItem(beginSlot.GetItem());

        isDraging = true;
    }

    public void EndDrag(ItemSlot slot)
    {
        endSlot = slot;
        EndDrag();
    }

    public void EndDrag()
    {
        if(endSlot != null && beginSlot != null)
        {
            if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.Inventory)
            {
                //Inventory To Inventory

                ItemSO temp = ghost.GetItem();

                beginSlot.SetItem(endSlot.GetItem());
                endSlot.SetItem(temp);
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.Storage)
            {
                //Inventory To Storage

                if (beginSlot.GetItem().itemId == (endSlot as StorageSlot).OriginItem.itemId)
                {
                    if (!StorageManager.Instance.IsItemFull(beginSlot.GetItem()))
                    {
                        SendManager.Instance.StorageDrop(beginSlot.GetItem().itemId);
                        beginSlot.SetItem(null);
                    }
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.ConverterBefore)
            {
                //Inventory To ConverterBefore

                if(beginSlot.GetItem() != null && beginSlot.GetItem().canRefining)
                {
                    ItemSO temp = null;

                    if (ConvertPanel.Instance.CurOpenConverter.IsConverting)
                    {
                        //NowConverting
                        temp = endSlot.GetItem();
                        SendManager.Instance.ResetConverter(ConvertPanel.Instance.CurOpenConverter.id);
                    }

                    SendManager.Instance.StartConverting(ConvertPanel.Instance.CurOpenConverter.id, beginSlot.GetItem().itemId);
                    beginSlot.SetItem(temp);
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.ConverterBefore && endSlot.Kind == ItemSlot.SlotKind.Inventory)
            {
                //ConverterBefore To Inventory

                ItemSO temp = beginSlot.GetItem();

                SendManager.Instance.ResetConverter(ConvertPanel.Instance.CurOpenConverter.id);
                endSlot.SetItem(temp);
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.ConverterAfter && endSlot.Kind == ItemSlot.SlotKind.Inventory)
            {
                //ConvertAfter To Inventory

                if(endSlot.IsEmpty)
                {
                    //endSlot is Empty

                    SendManager.Instance.TakeConverterAfterItem(ConvertPanel.Instance.CurOpenConverter.id);
                    endSlot.SetItem(beginSlot.GetItem());
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.MissionDropItem && endSlot.Kind == ItemSlot.SlotKind.Inventory)
            {
                //MissionItem to Inventory
                if(endSlot.IsEmpty)
                {
                    MissionDropItemSlot slot = beginSlot as MissionDropItemSlot;

                    if(slot.SlotMissionType == MissionType.Ore)
                    {
                        IMission mission = slot.GetComponentInParent<IMission>();
                        MissionOre oreMission = mission as MissionOre;

                        oreMission.OnGetItem();
                    }

                    endSlot.SetItem(beginSlot.GetItem());
                    
                    slot.Disable();
                }
            }
        }

        ghost.Init();

        beginSlot = null;
        endSlot = null;

        isDraging = false;

        return;
    }
}
