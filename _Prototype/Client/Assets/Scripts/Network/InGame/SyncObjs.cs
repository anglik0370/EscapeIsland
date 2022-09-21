using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType
{
    Converter = 0,
    Battery
}

public enum BehaviourType
{
    Start = 0,
    Reset,
    Take
}


public class SyncObjs : ISetAble
{
    SyncObjVO objVO;

    private bool needSyncRefresh = false;

    public void SetSyncObj(SyncObjVO objVO)
    {
        lock(lockObj)
        {
            this.objVO = objVO;
            needSyncRefresh = true;
        }
    }

    private void Update()
    {
        if(needSyncRefresh)
        {
            SyncObject();
            needSyncRefresh = false;
        }
    }

    public void SyncObject()
    {
        switch (objVO.objType)
        {
            case ObjType.Converter:
                SetConverter();
                break;
            case ObjType.Battery:
                SetBattery();
                break;
        }
    }

    public void SetConverter()
    {
        switch (objVO.behaviourType)
        {
            case BehaviourType.Start:
                SetStartConverter(objVO.data.objId, objVO.data.data);
                break;
            case BehaviourType.Reset:
                SetResetConverter(objVO.data.objId);
                break;
            case BehaviourType.Take:
                SetTakeConverterAfterItem(objVO.data.objId);
                break;
        }
    }

    public void SetBattery()
    {
        switch (objVO.behaviourType)
        {
            case BehaviourType.Start:
                SetChargingBattery(objVO.data.objId);
                break;
            case BehaviourType.Take:
                SetTakeBattery(objVO.data.objId);
                break;
        }
    }

    public void SetChargingBattery(int batteryChargerId)
    {
        ItemCharger charger = ChargerManager.Instance.FindChargerById(batteryChargerId);
        charger.StartCharging();

        if (objVO.userId.Equals(user.socketId))
        {
            user.inventory.slotList[objVO.slotIdx].SetItem(null);
        }
    }

    public void SetTakeBattery(int batteryChargerId)
    {
        ItemCharger charger = ChargerManager.Instance.FindChargerById(batteryChargerId);
        charger.Init();

        if (objVO.userId.Equals(user.socketId))
        {
            user.inventory.slotList[objVO.slotIdx].SetItem(ItemManager.Instance.FindItemSO(objVO.itemId));
        }
    }

    public void SetStartConverter(int converterId, int itemSOId)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(itemSOId);

        if(objVO.userId.Equals(user.socketId))
        {
            user.inventory.slotList[objVO.slotIdx].SetItem(null);
        }

        Debug.Log($"변환기{converterId}에서 {so}변환 시작");

        ConverterManager.Instance.ConverterList.Find(x => x.id == converterId).ConvertingStart(so,objVO.isImmediate);

        print("start");
    }

    public void SetResetConverter(int converterId)
    {
        ItemConverter converter = ConverterManager.Instance.ConverterList.Find(x => x.id == converterId);
        converter.ConvertingReset();

        if (objVO.userId.Equals(user.socketId))
        {
            user.inventory.slotList[objVO.slotIdx].SetItem(ItemManager.Instance.FindItemSO(objVO.itemId));
        }
        print("reset");
    }

    public void SetTakeConverterAfterItem(int converterId)
    {
        ItemConverter converter = ConverterManager.Instance.ConverterList.Find(x => x.id == converterId);
        converter.TakeIAfterItem();

        if (objVO.userId.Equals(user.socketId))
        {
            user.inventory.slotList[objVO.slotIdx].SetItem(ItemManager.Instance.FindItemSO(objVO.itemId));
        }
        //refinery.ingotItem = null;
        print("take");
    }
}
