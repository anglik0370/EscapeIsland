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

    public void EndDrag()
    {
        ghost.Init();

        beginSlot = null;
        endSlot = null;

        isDraging = false;
    }

    public void EndDrag(ItemSlot endSlot)
    {
        this.endSlot = endSlot;

        if (endSlot != null && beginSlot != null)
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

                //이제 저장소패널은 확인용으로만 사용한다

                //if (beginSlot.GetItem().itemId == (endSlot as StorageSlot).OriginItem.itemId)
                //{
                //    if (!StorageManager.Instance.IsItemFull(beginSlot.GetItem()))
                //    {
                //        SendManager.Instance.StorageDrop(beginSlot.GetItem().itemId);
                //        beginSlot.SetItem(null);
                //    }
                //}
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.ConverterBefore)
            {
                //Inventory To ConverterBefore
                if (ConvertPanel.Instance.CurOpenConverter.IsCanConvert(beginSlot.GetItem()))
                {
                    ItemSO temp = null;
                    SyncObjDataVO vo = new SyncObjDataVO(ConvertPanel.Instance.CurOpenConverter.id, beginSlot.GetItem().itemId);

                    if (ConvertPanel.Instance.CurOpenConverter.IsConverting)
                    {
                        //NowConverting
                        temp = endSlot.GetItem();

                        SendManager.Instance.SendSyncObj(vo, ObjType.Converter, BehaviourType.Reset);
                    }

                    SendManager.Instance.SendSyncObj(vo, ObjType.Converter, BehaviourType.Start);
                    beginSlot.SetItem(temp);
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.ConverterBefore && endSlot.Kind == ItemSlot.SlotKind.Inventory)
            {
                //ConverterBefore To Inventory

                ItemSO temp = beginSlot.GetItem();

                SyncObjDataVO vo = new SyncObjDataVO(ConvertPanel.Instance.CurOpenConverter.id, -1);
                SendManager.Instance.SendSyncObj(vo, ObjType.Converter, BehaviourType.Reset);
                endSlot.SetItem(temp);
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.ConverterAfter && endSlot.Kind == ItemSlot.SlotKind.Inventory)
            {
                //ConvertAfter To Inventory

                if(endSlot.IsEmpty)
                {
                    //endSlot is Empty
                    SyncObjDataVO vo = new SyncObjDataVO(ConvertPanel.Instance.CurOpenConverter.id, -1);
                    SendManager.Instance.SendSyncObj(vo, ObjType.Converter, BehaviourType.Take);
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
                        IGetMission mission = slot.GetComponentInParent<IGetMission>();
                        MissionOre oreMission = mission as MissionOre;

                        oreMission.OnGetItem();
                    }

                    endSlot.SetItem(beginSlot.GetItem());
                    
                    slot.Disable();
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.MissionBatterySlot)
            {
                //Inventory To Charge

                MissionBatterySlot slot = endSlot as MissionBatterySlot;

                if(beginSlot.GetItem() == slot.EmptyBatterySO && slot.IsEmpty && !slot.MissionCharge.IsCharging)
                {
                    SyncObjDataVO vo = new SyncObjDataVO(slot.MissionCharge.CurOpenCharger.Id, -1);
                    SendManager.Instance.SendSyncObj(vo, ObjType.Battery, BehaviourType.Start);
                    //slot.SetEmptyBetteryItem();
                    //slot.StartCharging();
                    beginSlot.SetItem(null);
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.MissionBatterySlot && endSlot.Kind == ItemSlot.SlotKind.Inventory)
            {
                //Charge To Inventory

                MissionBatterySlot slot = beginSlot as MissionBatterySlot;

                if(endSlot.IsEmpty && slot.IsMaxCharge)
                {

                    SyncObjDataVO vo = new SyncObjDataVO(slot.MissionCharge.CurOpenCharger.Id, -1);
                    SendManager.Instance.SendSyncObj(vo, ObjType.Battery, BehaviourType.Take);

                    //slot.SetNullItem();

                    //slot.InitCurCharger();
                    endSlot.SetItem(slot.BatterySO);
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.CantUseRefinery)
            {
                if(endSlot.IsEmpty && beginSlot.GetItem() == ConvertPanel.Instance.SandItem)
                {

                    beginSlot.SetItem(null);
                    //endSlot.SetItem(ConvertPanel.Instance.SandItem);

                    CantUseRefineryVO vo = new CantUseRefineryVO(ConvertPanel.Instance.CurOpenConverter.id, ConvertPanel.Instance.GetRefinerySlotIdx(endSlot));

                    SendManager.Instance.SendCantUseRefinery(vo);
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.Arson)
            {
                if(endSlot.IsEmpty && ArsonManager.Instance.CanExtinguish(beginSlot.GetItem()))
                {
                    ArsonSlot slot = endSlot as ArsonSlot;

                    beginSlot.SetItem(null);

                    ArsonVO vo = new ArsonVO(slot.id, ArsonManager.Instance.AllExtinguish(slot.id));

                    SendManager.Instance.SendExtinguish(vo);
                }
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.TrashCan)
            {
                beginSlot.SetItem(null);
            }
            else if(beginSlot.Kind == ItemSlot.SlotKind.Inventory && endSlot.Kind == ItemSlot.SlotKind.MissionStorageDropSlot)
            {
                if(ghost.GetItem() == endSlot.GetItem()) //아이템이 일치할 경우
                {
                    MSSlot msSlot = endSlot as MSSlot;
                    IStorageMission mission = msSlot.Mission;

                    int curItemCount = StorageManager.Instance.FindItemAmount(false, mission.Team, mission.StorageItem).amount;
                    int maxItemCount = StorageManager.Instance.FindItemAmount(true, mission.Team, mission.StorageItem).amount;

                    if (StorageManager.Instance.IsItemFull(mission.Team))
                    {
                        //꽉찼으니 꽉찼다고 서버에 보내줘야 한다.
                        TeamVO vo = new TeamVO(mission.Team);
                        DataVO dataVO = new DataVO("STORAGE_FULL", JsonUtility.ToJson(vo));

                        Debug.Log("꽉참");

                        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
                        return;
                    }

                    if (curItemCount < maxItemCount)
                    {
                        if (msSlot.IsEmpty)
                        {
                            //여기서 저장소로 날려주면 됨
                            SendManager.Instance.StorageDrop(mission.Team, beginSlot.GetItem().itemId);
                            beginSlot.SetItem(null);
                        }
                    }
                }
            }
            else
            {
                print($"Begin = {beginSlot.transform.gameObject.name}, End = {endSlot.transform.gameObject.name}");
            }
        }

        ghost.Init();

        beginSlot = null;
        endSlot = null;

        isDraging = false;
    }
}
