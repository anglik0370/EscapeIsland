const WebSocket = require('ws');

const {Users} = require('./Users.js');
const SetSpawnPoint = require('./Utils/GameSpawnHandler.js');
const GetRandomPos = require('./Utils/SpawnPoint.js');
const SocketState = require('./Utils/SocketState.js');
const team = require('./Utils/Team.js');
const MissionType = require('./Utils/MissionType.js');
const AreaState = require('./Utils/AreaState.js');
const BehaviourType = require('./Utils/BehaviourType.js');
const sendError = require('./Utils/SendError.js');

const values = Object.values(MissionType);

const Timer = require('./Timers/Timer.js');
const InGameTimer = require('./Timers/InGameTimer.js');

const Area = require('./Area.js');
const Storage = require('./Utils/Storage.js');

class Room {
    constructor(roomName,roomNum,curUserNum,userNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.playing = playing;

        this.inGameTimer = new InGameTimer(this,20,() => {});
        this.altarTimer = new Timer(this,30,() => {});

        this.skipCount = 0;
        this.isOnce = false;

        this.socketList = [];

        this.storageItemList = {};
        this.areaList = {};
        this.selectedIdList = {};
        this.userList = {};
        this.redSpawnerList = {};
        this.blueSpawnerList = {};
        this.syncObjList = {};

        this.initSpawnerList();
        this.initSelectedIdList();
        this.initAreaList();
        this.initStorageItemList();
    }

    initSyncObjList() {
        for(let key in this.syncObjList) {
            this.syncObjList[key] = false;
        }
    }

    initStorageItemList() {
        this.storageItemList[team.RED] = new Storage();
        this.storageItemList[team.BLUE] = new Storage();
    }

    initAreaList() {
        for(let key in this.areaList) {
            this.areaList[key].initTimer();
        }

        this.areaList[AreaState.Cave] = new Area(AreaState.Cave, "동굴",this);
        this.areaList[AreaState.Beach] = new Area(AreaState.Beach,"모래사장",this);
        this.areaList[AreaState.Field] = new Area(AreaState.Field,"밭",this);
        this.areaList[AreaState.Forest] = new Area(AreaState.Forest,"숲",this);
    }

    initSpawnerList() {
        for(let i = 0; i < values.length; i++) {
            this.redSpawnerList[values[i]] = { };
            this.blueSpawnerList[values[i]] = { };
        }
    }

    initSelectedIdList() {
        let values = Object.values(team);
        for(let i = 0; i < values.length; i++) {
            this.selectedIdList[values[i]] = [];
        }
    }

    getSelectedIdList(team) {
        return this.selectedIdList[team];
    }

    setSpawnerData(socket,data) {
        let value = data.team == team.RED ? this.redSpawnerList[data.missionType] : this.blueSpawnerList[data.missionType];

        if(!data.isOpen) {
            value[data.spawnerId] = data.isOpen;
            return;
        }

        if(this.areaList[data.area] === undefined) {
            socket.send(JSON.stringify({type:"OPEN_MISSION",payload:null}));
            return;
        }

        if(!this.areaList[data.area].canMission(this.userList[socket.id],data.isGathering)) {
            sendError("현재 이 미션을 이용하실 수 없습니다.", socket);
            return;
        }

        if(data.isOpen && value[data.spawnerId] !== undefined) {
            if(value[data.spawnerId]) {
                sendError(`누군가 이 미션을 하는 중입니다 ${data.spawnerId}`,socket);
                return;
            }
        }

        value[data.spawnerId] = data.isOpen;

        //send
        socket.send(JSON.stringify({type:"OPEN_MISSION",
        payload:null}));
    }

    altar(socket,data) {
        if(this.altarTimer.isTimer) {
            sendError("쿨타임 도는중",socket);
            return;
        }

        this.broadcast(JSON.stringify({type:"ALTAR",
        payload:JSON.stringify(data)}))
        this.altarTimer.startTimer(true);
    }

    addVoiceData(socketId,voiceData) {
        this.userList[socketId].setVoiceData(voiceData);
    }

    initVoiceData() {
        for(let key in this.userList) {
            this.userList[key].voiceData = [];
        }
    }

    changeCharacter(team,beforeId,characterId) {
        this.selectedIdList[team] = this.selectedIdList[team].filter(id => id !== beforeId);

        if(characterId <= 0) return;

        this.selectedIdList[team].push(characterId);
    }

    syncObjRefresh(data) {
        let objId = data.data.objId;

        if(this.syncObjList[objId] !== undefined) {
            if((data.behaviourType == BehaviourType.Reset || data.behaviourType == BehaviourType.Take)) {
                if(!this.syncObjList[objId]) return;

                this.syncObjList[objId] = false;
            }
            else {
                if(this.syncObjList[objId]) return;
            }
        }
        
        if(data.behaviourType == BehaviourType.Start) {
            this.syncObjList[objId] = true;
        }

        this.broadcast(JSON.stringify({type:"SYNC_OBJ", payload:JSON.stringify(data)}));
    }

    areaRefresh(socket,areaState) {
        let user = this.userList[socket.id];

        if(user !== undefined) {

            let isBlue = user.curTeam == team.BLUE;
            if(user.area == areaState) {
                //구역이 같음
                return;
            }

            if(this.areaList[user.area] !== undefined) {
                this.areaList[user.area].removeUserList(isBlue, user.charId == 7);
                socket.send(JSON.stringify({type:"REFRESH_AREA",payload:JSON.stringify({isOpen:false})}))
            }

            this.userList[socket.id].area = areaState;

            if(this.areaList[areaState] !== undefined) {
                this.areaList[areaState].addUserList(isBlue, user.charId == 7);
            }
        }
    }

    setSpawnPos() {
        let redTeamList = [];
        let blueTeamList = [];

        for(let key in this.userList) {
            if(this.userList[key].curTeam == team.NONE) continue;

            if(this.userList[key].curTeam == team.RED) {
                redTeamList.push(this.userList[key]);
            }
            else {
                blueTeamList.push(this.userList[key]);
            }
        }

        let redTeamPos = SetSpawnPoint(redTeamList.length,false);
        let blueTeamPos = SetSpawnPoint(blueTeamList.length,true);

        for(let i = 0; i < redTeamList.length; i++) {
            redTeamList[i].position = redTeamPos[i];
        }

        for(let i = 0; i < blueTeamList.length; i++) {
            blueTeamList[i].position = blueTeamPos[i];
        }
    }

    allReady() {
        // for(let key in this.userList) {
        //     if(this.userList[key].socketId >= 1000) {
        //         return true;
        //     }
        // }

        for(let key in this.userList) {
            if(!this.userList[key].ready && !this.userList[key].master) {
                return false;
            }
        }

        return true;
    }

    gameStart(socket,data) {
        if(socket.state !== SocketState.IN_ROOM){
            sendError("방이 아닌 곳에서 시도를 하였습니다.", socket);
            return;
        }
    
        if(this.curUserNum < 2) {
            sendError("최소 2명 이상의 인원이 있어야 합니다.",socket);
            return;
        }

        if(this.playing) {
            sendError("현재 방은 이미 시작되었습니다.",socket);
            return;
        }

        if(!this.allReady()) {
            sendError("모든 사람이 준비하지 않았습니다.",socket);
            return;
        }

        //게임 데이터 세팅

        if(!this.isOnce) {
            for(let i = 0; i < data.itemAmountList.length; i++) {
                let itemAmount = data.itemAmountList[i];
    
                this.storageItemList[team.RED].setItemAmount(itemAmount.itemId,itemAmount.amount);
                this.storageItemList[team.BLUE].setItemAmount(itemAmount.itemId,itemAmount.amount);
            }

            this.isOnce = true;
        }
        else {
            this.storageItemList[team.RED].initStorage();
            this.storageItemList[team.BLUE].initStorage();
        }
        

        this.setSpawnPos();
        
        let dataList = this.getUsersData();
    
        this.playing = true;
        this.startTimer(true);
        this.broadcast(JSON.stringify({type:"GAME_START",payload:JSON.stringify({dataList})}));
    }
    
    initRoom() {
        console.log("initRoom");

        this.playing = false;
        this.skipCount = 0;

        for(let key in this.userList) {
            this.userList[key].isInside = false;
            this.userList[key].ready = false;
        }
        
        this.inGameTimer.stopTimer(true);
        this.altarTimer.stopTimer(true);
        
        this.initSyncObjList();
        //this.initStorageItemList();
        this.initAreaList();
        //his.initSelectedIdList();
        this.initSpawnerList();
    }

    storageDrop(data) {
        let isFull = this.storageItemList[data.team].addItemAmount(data.itemSOId);

        data.isFull = isFull;
        this.broadcast(JSON.stringify({type:"STORAGE_DROP",payload:JSON.stringify(data)}));

        if(this.storageItemList[data.team].IsFullStorage()) {
            this.gameEnd(data.team);
        }
    }

    gameEnd(winTeam) {
        //this.setSpawnPos();
        for(let key in this.userList) {
            this.userList[key].position = GetRandomPos();
        }
    
        let dataList = this.getUsersData();
        this.broadcast(JSON.stringify({type:"WIN",payload:JSON.stringify({dataList,gameOverCase:winTeam})}),true);
        this.initRoom();
    }

    timerEnd() {
        let redAmount = this.storageItemList[team.RED].totalCollectedItemAmount;
        let blueAmount = this.storageItemList[team.BLUE].totalCollectedItemAmount;

        let winTeam = team.NONE;

        if(redAmount == blueAmount) {
            winTeam = team.NONE;
        }
        else {
            winTeam = redAmount > blueAmount ? team.RED : team.BLUE
        }

        this.gameEnd(winTeam);
    }

    setTimersTime(socket){
        if(socket.readyState !== WebSocket.OPEN) return;
        
        socket.send(JSON.stringify({type:"SET_TIME",payload:JSON.stringify({inGameTime:this.inGameTimer.maxTime})}));
    }

    startTimer(isInit) {
        this.inGameTimer.startTimer(isInit);
        this.broadcast(JSON.stringify({type:"TIMER",payload:JSON.stringify({type:"IN_GAME",isStart:true})}));

        let key = Object.keys(this.areaList);

        for(let i = 0; i < key.length; i++) {
            this.areaList[key[i]].startTimer();
        }
    }

    addSocket(socket,user) {
        this.socketList.push(socket);
        this.userList[user.socketId] = user;
        this.curUserNum++;
    }

    removeSocket(rSocketIdx) {
        let idx = this.socketList.findIndex(soc => soc.id == rSocketIdx);
        if(idx > -1)
            this.socketList.splice(idx,1);

        let user = this.userList[rSocketIdx];
        let index = this.selectedIdList[user.curTeam].findIndex(id => id == user.charId);
        if(index > -1)
            this.selectedIdList[user.curTeam].splice(index,1);

        delete this.userList[rSocketIdx];
    }

    refreshUserCount() {
        if(this.playing) return;

        this.broadcast(JSON.stringify({type:"REFRESH_LOBBY_UI",payload:JSON.stringify({roomVO:this.returnData(),dataList:this.getUsersData()})}));
    }

    returnData() {
        let data = {name:this.roomName,roomNum:this.roomNum,curUserNum:this.curUserNum,userNum:this.userNum,kidnapperNum:this.kidnapperNum,playing:this.playing};
        return data;
    }

    getAreaListData() {
        let areaDataList = [];

        for(let key in this.areaList) {
            areaDataList.push(this.areaList[key].getPayload());
        }

        return areaDataList;
    }

    getUsersData() {
        return Object.values(this.userList);
    }

    broadcast(msg,isEnd = false) {
        this.socketList.forEach(soc => {
            if(soc.readyState !== WebSocket.OPEN) return;

            if(isEnd) soc.state = SocketState.IN_ROOM;
            soc.send(msg);
        });
    }

    teamBroadcast(msg,team,isSame) {
        if(isSame) {
            this.socketList.forEach(soc => {
                let user = this.userList[soc.id];
    
                if(user.curTeam === team) {
                    soc.send(msg);
                }
            });
        }
        else {
            this.socketList.forEach(soc => {
                let user = this.userList[soc.id];

                if(user.curTeam !== team) {
                    soc.send(msg);
                }
            });
        }
        
    }

}

module.exports = Room;