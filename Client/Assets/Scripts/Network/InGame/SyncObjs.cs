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

    public void SetStartConverter(int converterId, int itemSOId)
    {
        ItemSO so = ItemManager.Instance.FindItemSO(itemSOId);

        Debug.Log($"변환기{converterId}에서 {so}변환 시작");

        ConverterManager.Instance.ConverterList.Find(x => x.id == converterId).ConvertingStart(so);

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
