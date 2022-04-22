class SyncObjVO {
    constructor(objType,behaviourType,objData) {
        this.objType = objType;
        this.behaviourType = behaviourType;
        this.objData = objData;
    }
}

class SyncObjDataVO {
    constructor(objId,data) {
        this.objId = objId;
        this.data = data;
    }
}

module.exports = {SyncObjVO,SyncObjDataVO};