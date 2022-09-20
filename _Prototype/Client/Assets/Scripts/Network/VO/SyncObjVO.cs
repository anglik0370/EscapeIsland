[System.Serializable]
public class SyncObjVO : VO
{
    public bool isImmediate;
    public ObjType objType;
    public BehaviourType behaviourType;
    public SyncObjDataVO data;

    public SyncObjVO()
    {

    }

    public SyncObjVO(bool isImmediate, ObjType objType, BehaviourType behaviourType, SyncObjDataVO data)
    {
        this.isImmediate = isImmediate;
        this.objType = objType;
        this.behaviourType = behaviourType;
        this.data = data;
    }
}

[System.Serializable]
public class RefineryVO : VO
{
    public int refineryId;
    public int itemSOId;

    public RefineryVO()
    {

    }

    public RefineryVO(int refineryId, int itemSOId)
    {
        this.refineryId = refineryId;
        this.itemSOId = itemSOId;
    }
}

[System.Serializable]
public class SyncObjDataVO : VO
{
    public int objId;
    public int data;

    public SyncObjDataVO(int objId, int data)
    {
        this.objId = objId;
        this.data = data;
    }
}