const {Users} = require('./Users.js');
const Timer = require('./Timers/Timer.js');
const InGameTimer = require('./Timers/InGameTimer.js');
const SetSpawnPoint = require('./Utils/GameSpawnHandler.js');
const SocketState = require('./Utils/SocketState.js');
const WebSocket = require('ws');
const sendError = require('./Utils/SendError.js');

class Room {
    constructor(roomName,roomNum,curUserNum,userNum,kidnapperNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.kidnapperNum = kidnapperNum;
        this.playing = playing;

        this.inGameTimer = new InGameTimer(this,20,() => this.inGameTimerCallback());
        this.arsonTimer = new Timer(this,40, () => this.sendKidnapperWin(0));
        this.inVoteTimer = new Timer(this,180,() => this.voteTimerCallBack());
        this.isEndGame = false;

        this.skipCount = 0;
        
        this.interval = 1000;
        this.nextTime = 0;
        this.expected = Date.now();

        this.socketList = [];
        this.userList = {};
        this.selectedIdList = [];
    }

    changeCharacter(beforeId,characterId) {
        this.selectedIdList = this.selectedIdList.filter(id => id !== beforeId);

        this.selectedIdList.push(characterId);
    }

    areaRefresh(socket,areaState) {
        if(this.userList[socket.id] !== undefined) {
            this.userList[socket.id].areaState = areaState;
        }

        let dataList = this.getUsersData();

        this.broadcast(JSON.stringify({type:"INSIDE_REFRESH",payload:JSON.stringify({dataList})}));
    }

    voteEnd() {
        let allComplete = true;
        let targetSocIdArr = [];

        let isTest = false;

        for(let key in this.userList) {
            if(key >= 1000) {
                isTest = true;
                break;
            }
        }

        for(let key in this.userList) {
            if(!this.userList[key].isDie && !this.userList[key].voteComplete) {
                allComplete = false;
                break;
            }
        }
        
        if(allComplete || isTest) {
            let dummy = -1;
    
            for(let key in this.userList) {
                let user = this.userList[key];
                let idx = user.voteNum;
    
                if(dummy != 0 &&  idx == dummy) {
                    targetSocIdArr.push(user.socketId);
                }
                else if(idx > dummy && idx > this.skipCount) {
                    dummy = idx;
                    targetSocIdArr.length = 0;
                    targetSocIdArr.push(user.socketId);
                }
    
                this.userList[key].voteNum = 0;
                this.userList[key].voteComplete = false;
            }
            this.skipCount = 0;
            
            if(targetSocIdArr.length == 1) {
                this.broadcast(JSON.stringify({type:"VOTE_DIE",payload:targetSocIdArr[0]}));
                this.userList[targetSocIdArr[0]].isDie = true;
    
                //납치자를 모두 찾았을때
    
                this.setSpawnPos();
                
                let dataList = this.getUsersData();
                let filteredArr = dataList.filter(user => user.isImposter && !user.isDie);
    
                if(filteredArr.length <= 0) {
                    this.broadcast(JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:1})}),true);
                    this.initRoom();
                    return true;
                }

                if(this.kidnapperWinCheck()) {
                    this.initRoom();
                    return true;
                }
    
            }
            this.voteTimeEnd();
            return true;
        }
        return false;
    }

    endGameHandle(goc) {
        if(goc > 2) return;

        this.setSpawnPos();
        
        let dataList = this.getUsersData();

        this.broadcast(goc <= 0 ? JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList,gameOverCase:goc})})
        : JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:goc})}),true);
        this.initRoom();
    }

    voteTimeEnd() {
        this.startTimer(false);
    }

    kidnapperWinCheck() {
        let imposterCount = 0;
        let citizenCount = 0;
    
    
        for(let key in this.userList) {
            if(this.userList[key].isDie) continue;
    
            if(this.userList[key].isImposter) imposterCount++;
            else citizenCount++;
        }
    
        this.setSpawnPos();
    
        //살아있는 임포가 시민보다 많을 경우
        if(imposterCount >= citizenCount) {
            //임포승
            this.sendKidnapperWin(0);
            this.initRoom();
            return true;
        }
        return false;
    }

    setSpawnPos() {
        let keys = Object.keys(this.userList);
        let posList = SetSpawnPoint(keys.length);
    
        for(let i = 0; i < keys.length; i++) {
            this.userList[keys[i]].position = posList[i];
        }
    }

    sendKidnapperWin(gameOverCase) {
        this.setSpawnPos();
        let dataList = this.getUsersData();

        this.broadcast(JSON.stringify({type:"WIN_KIDNAPPER",
            payload:JSON.stringify({dataList,gameOverCase})}),true);
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

        let isTest = false;

        for(let key in this.userList) {
            if(this.userList[key].socketId >= 1000) {
                isTest = true;
                break;
            }
        }

        let keys = Object.keys(this.userList);
        let imposterLength = this.kidnapperNum;
        let idx;
    
        if(isTest) {
            for(let key in this.userList) {
                if(this.userList[key].master) {
                    this.userList[key].isImposter = true;
                    break;
                }
            }
        }
        else {
            for(let i = 0; i < imposterLength; i++) {
                do {
                    idx = Math.floor(Math.random() * keys.length);
                }while(this.userList[keys[idx]].isImposter)
        
                this.userList[keys[idx]].isImposter = true;
            }
        }
        
    
        let posList = SetSpawnPoint(keys.length);
    
        for(let i = 0; i < keys.length; i++) {
            this.userList[keys[i]].position = posList[i];
        }
        
        let dataList = this.getUsersData();
    
        this.playing = true;
        this.startTimer(true);
        this.broadcast(JSON.stringify({type:"GAME_START",payload:JSON.stringify({dataList})}));
    }
    
    initRoom() {
        console.log("initRoom");
        this.playing = false;
        this.skipCount = 0;

        if(Users.isTestServer) {
            this.inVoteTimer.setMaxTime(10);
        }

        for(let key in this.userList) {
            this.userList[key].isDie = false;
            this.userList[key].isImposter = false;
            this.userList[key].voteNum = 0;
            this.userList[key].voteComplete = false;
            this.userList[key].isInside = false;
        }
        
        this.inGameTimer.stopTimer();
        this.arsonTimer.stopTimer();
        this.inVoteTimer.stopTimer();
    }

    setTimersTime(socket){
        if(socket.readyState !== WebSocket.OPEN) return;
        
        socket.send(JSON.stringify({type:"SET_TIME",payload:JSON.stringify
        ({inGameTime:this.inGameTimer.maxTime, voteTime:this.inVoteTimer.maxTime})}));
    }

    startTimer(isInit) {
        this.inVoteTimer.stopTimer();
        this.inGameTimer.startTimer(isInit);
        this.broadcast(JSON.stringify({type:"TIMER",payload:JSON.stringify({type:"IN_GAME",isStart:true})}));
    }

    startVoteTimer() {
        this.inGameTimer.stopTimer();
        this.broadcast(JSON.stringify({type:"TIMER",payload:JSON.stringify({type:"IN_VOTE",isStart:true})}));
        this.inVoteTimer.startTimer(true);
    }

    inGameTimerCallback() {
        if(this.isEndGame) {

            this.setSpawnPos();
    
            let dataList = this.getUsersData();

            this.broadcast(JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:2})}),true);
            this.initRoom();
            return;
        }
    }

    voteTimerCallBack() {
        if(!this.voteEnd()) {
            this.voteTimeEnd();
        }
    }

    addSocket(socket,user) {
        this.socketList.push(socket);
        this.userList[user.socketId] = user;
        this.curUserNum++;
    }

    removeSocket(rSocketIdx) {
        let idx = this.socketList.findIndex(soc => soc.id == rSocketIdx);
        this.socketList.splice(idx,1);
        delete this.userList[rSocketIdx];
    }

    returnData() {
        let data = {name:this.roomName,roomNum:this.roomNum,curUserNum:this.curUserNum,userNum:this.userNum,kidnapperNum:this.kidnapperNum,playing:this.playing};
        return data;
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

}

module.exports = Room;