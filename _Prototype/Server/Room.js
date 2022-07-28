const WebSocket = require('ws');

const {Users} = require('./Users.js');
const SetSpawnPoint = require('./Utils/GameSpawnHandler.js');
const SocketState = require('./Utils/SocketState.js');
const team = require('./Utils/Team.js');
const MissionType = require('./Utils/MissionType.js');
const AreaState = require('./Utils/AreaState.js');
const sendError = require('./Utils/SendError.js');

const values = Object.values(MissionType);

const Timer = require('./Timers/Timer.js');
const InGameTimer = require('./Timers/InGameTimer.js');
const ArsonTimer = require('./Timers/ArsonTimer.js');

const Area = require('./Area.js');

class Room {
    constructor(roomName,roomNum,curUserNum,userNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.playing = playing;

        this.inGameTimer = new InGameTimer(this,20,() => {});
        this.arsonTimer = new ArsonTimer(this,20, () => this.arsonCallback());

        this.skipCount = 0;

        this.socketList = [];
        this.storageItemList = [];

        this.areaList = {};
        this.selectedIdList = {};
        this.userList = {};
        this.redSpawnerList = {};
        this.blueSpawnerList = {};

        this.initSpawnerList();
        this.initSelectedIdList();
        this.initAreaList();
    }

    initAreaList() {
        this.areaList[AreaState.Cave] = new Area(AreaState.Cave, "동굴");
        this.areaList[AreaState.Beach] = new Area(AreaState.Beach,"모래사장");
        this.areaList[AreaState.Field] = new Area(AreaState.Field,"밭");
        this.areaList[AreaState.Forest] = new Area(AreaState.Forest,"숲");
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

        if(data.isOpen && value[data.spawnerId] !== undefined) {
            if(value[data.spawnerId]) {
                sendError("누군가 이 미션을 하는 중입니다",socket);
                return;
            }
        }

        value[data.spawnerId] = data.isOpen;

        //send
        if(!data.isOpen) return;

        socket.send(JSON.stringify({type:"OPEN_MISSION",
        payload:null}));
    }

    arsonCallback() {
        if(this.storageItemList.length <= 0)  {
            return;
        }

        let idx = Math.floor(Math.random() * this.storageItemList.length);
        let itemSOId = this.storageItemList[idx];

        this.storageItemList.splice(idx,1);

        this.broadcast(JSON.stringify({type:"ARSON",
        payload:JSON.stringify({team:this.arsonTimer.team,itemSOId})}));
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

    areaRefresh(socket,areaState) {
        let user = this.userList[socket.id];

        if(user !== undefined) {

            let isBlue = user.curTeam == team.BLUE;

            if(user.areaState == areaState) {
                //구역이 같음
                return;
            }

            if(this.areaList[user.areaState] !== undefined) {
                this.areaList[user.areaState].removeUserList(isBlue);
                socket.send(JSON.stringify({type:"REFRESH_AREA",payload:JSON.stringify({isOpen:false})}))
            }

            this.userList[socket.id].areaState = areaState;

            if(this.areaList[areaState] !== undefined) {
                this.areaList[areaState].addUserList(isBlue);
            }
        }
    }

    setSpawnPos() {
        let keys = Object.keys(this.userList);
        let posList = SetSpawnPoint(keys.length);
    
        for(let i = 0; i < keys.length; i++) {
            this.userList[keys[i]].position = posList[i];
        }
    }

    allReady() {
        for(let key in this.userList) {
            if(this.userList[key].socketId >= 1000) {
                return true;
            }
        }

        for(let key in this.userList) {
            if(!this.userList[key].ready && !this.userList[key].master) {
                return false;
            }
        }

        return true;
    }

    gameStart(socket) {
        if(socket.state !== SocketState.IN_ROOM){
            sendError("방이 아닌 곳에서 시도를 하였습니다.", socket);
            return;
        }
    
        if(this.curUserNum < 2) {
            sendError("최소 2명 이상의 인원이 있어야 합니다.",socket);
            return;
        }
    
        if(this.curUserNum <= this.kidnapperNum) {
            sendError("현재 유저의 수가 납치자의 수보다 같거나 적습니다.",socket);
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
        this.arsonTimer.stopTimer(true);

        let key = Object.keys(this.areaList);

        for(let i = 0; i < key.length; i++) {
            this.areaList[key[i]].initTimer();
        }

        this.storageItemList = [];
        this.initSelectedIdList();
        this.initSpawnerList();
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