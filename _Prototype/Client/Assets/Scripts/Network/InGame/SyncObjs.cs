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
    }

    public void SetTakeBattery(int batteryChargerId)
    {
        ItemCharger charger = ChargerManager.Instance.FindChargerById(batteryChargerId);
        charger.Init();
    }

    public void SetStartConverter(int converterId, int itemSOId)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(itemSOId);

        Debug.Log($"??????{converterId}???? {so}???? ????");

        ConverterManager.Instance.ConverterList.Find(x => x.id == converterId).ConvertingStart(so,objVO.isImmediate);

        print("start");
    }

    public void SetResetConverter(int converterId)
    {
        ItemConverter converter = ConverterManager.Instance.ConverterList.Find(x => x.id == converterId);
        converter.ConvertingReset();
        print("reset");
    }

    public void SetTakeConverterAfterItem(int converterId)
    {
        ItemConverter converter = ConverterManager.Instance.ConverterList.Find(x => x.id == converterId);
        converter.TakeIAfterItem();
        //refinery.ingotItem = null;
        print("take");
    }
}
