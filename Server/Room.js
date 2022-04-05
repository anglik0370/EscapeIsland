//const {Rooms} = require('./Rooms.js');
const {Users} = require('./Users.js');
const InGameTimer = require('./InGameTimer.js');
const InVoteTimer = require('./InVoteTimer.js');
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

        this.inGameTimer = new InGameTimer();
        this.inVoteTimer = new InVoteTimer();
        this.curTimer = undefined;

        this.skipCount = 0;
        
        this.interval = 1000;
        this.nextTime = 0;
        this.expected = Date.now();

        this.socketList = [];
        this.userList = {};

        this.isInitRoom = false;
    }

    voteEnd() {
        let keys = Object.keys(this.userList);
        let allComplete = true;
        let targetSocIdArr = [];

        let isTest = false;

        for(let key in this.userList) {
            console.log(key);
            if(key >= 1000) {
                isTest = true;
                break;
            }
        }
        
        for(let i = 0; i < keys.length; i++) {
            //안죽었을때 & 투표완료했을때 넘어가야함
            if((!this.userList[keys[i]].isDie && !this.userList[keys[i]].voteComplete)) {
                allComplete = false;
                break;
            }
        }
        
        if(allComplete || isTest) {
            let dummy = -1;
    
            for(let i = 0; i < keys.length; i++) {
                let user = this.userList[keys[i]];
                let idx = user.voteNum;
    
                if(dummy != 0 &&  idx == dummy) {
                    targetSocIdArr.push(user.socketId);
                }
                else if(idx > dummy && idx > this.skipCount) {
                    dummy = idx;
                    targetSocIdArr.length = 0;
                    targetSocIdArr.push(user.socketId);
                }
    
                this.userList[keys[i]].voteNum = 0;
                this.userList[keys[i]].voteComplete = false;
            }
            this.skipCount = 0;
            this.inVoteTimer.initTime();
            
            if(targetSocIdArr.length == 1) {
                this.broadcast(JSON.stringify({type:"VOTE_DIE",payload:targetSocIdArr[0]}));
                this.userList[targetSocIdArr[0]].isDie = true;
    
                //납치자를 모두 찾았을때
    
                let keys = Object.keys(this.userList);
                let posList = SetSpawnPoint(keys.length);
    
                for(let i = 0; i < keys.length; i++) {
                    this.userList[keys[i]].position = posList[i];
                }
                
                let dataList = Object.values(this.userList);
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

    voteTimeEnd() {
        //this.broadcast(JSON.stringify({type:"VOTE_TIME_END",payload:""}));
        this.startTimer();
    }

    kidnapperWinCheck() {
        let imposterCount = 0;
        let citizenCount = 0;
    
        let keys = Object.keys(this.userList);
    
        for(let i = 0; i < keys.length; i++) {
            if(this.userList[keys[i]].isDie) continue;
    
            if(this.userList[keys[i]].isImposter) imposterCount++;
            else citizenCount++;
        }
    
        let posList = SetSpawnPoint(keys.length);
    
        for(let i = 0; i < keys.length; i++) {
            this.userList[keys[i]].position = posList[i];
        }
        let dataList = Object.values(this.userList);
    
        //살아있는 임포가 시민보다 많을 경우
        if(imposterCount >= citizenCount) {
            //임포승
            console.log("다주것다");
            this.broadcast(JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList,gameOverCase:0})}),true);
            this.initRoom();
            return true;
        }
        console.log("아직 남았다");
        return false;
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
    
        let keys = Object.keys(this.userList);
        let imposterLength = this.kidnapperNum;
        let idx;
    
        for(let i = 0; i < imposterLength; i++) {
            do {
                idx = Math.floor(Math.random() * keys.length);
            }while(this.userList[keys[idx]].isImposter)
    
            this.userList[keys[idx]].isImposter = true;
        }
    
        //Rooms.roomBroadcast(this.roomNum);
        //let d = Object.values(this.userList);
        //this.broadcast(JSON.stringify({type:"REFRESH_MASTER",payload:JSON.stringify({dataList:d})}));
    
        //룸에 있는 플레이어들의 포지션 조정
    
        let posList = SetSpawnPoint(keys.length);
    
        for(let i = 0; i < keys.length; i++) {
            this.userList[keys[i]].position = posList[i];
        }
        
        let dataList = Object.values(this.userList);
    
        this.isInitRoom = false;
        this.playing = true;
        this.startTimer();
        this.broadcast(JSON.stringify({type:"GAME_START",payload:JSON.stringify({dataList})}));
    }
    
    initRoom() {
        console.log("initRoom");
        this.playing = false;
        this.isInitRoom = true;
        this.stopTimer();
        this.skipCount = 0;
        this.inGameTimer = new InGameTimer();
        this.inVoteTimer = new InVoteTimer();

        if(Users.isTestServer) {
            this.roomList[this.roomNum].inVoteTimer.setTimeToNextSlot(10);
        }

        for(let key in this.userList) {
            this.userList[key].isDie = false;
            this.userList[key].isImposter = false;
            this.userList[key].voteNum = 0;
            this.userList[key].voteComplete = false;
        }

    }

    startTimer() {
        //this.skipCount = 0;
        console.log("startInGameTImer");
        this.stopTimer();
        this.expected = Date.now() + 1000; //현재시간 + 1초
        this.curTimer = setTimeout(this.rTimer.bind(this),this.interval);
        this.broadcast(JSON.stringify({type:"TIMER",payload:JSON.stringify({type:"IN_GAME",isStart:true})}));
    }

    startVoteTimer() {
        //this.inVoteTimer.initTime();
        this.stopTimer();
        this.curTimer = setTimeout(this.voteTimer.bind(this),this.interval);
        this.broadcast(JSON.stringify({type:"TIMER",payload:JSON.stringify({type:"IN_VOTE",isStart:true})}));
    }

    stopTimer() {
        clearTimeout(this.curTimer);
    }

    rTimer() {
        let dt = Date.now() - this.expected; //현재 시간 - 시작시간

        this.inGameTimer.timeRefresh(this.socketList)

        this.expected += this.interval;

        this.nextTime = Math.max(0,this.interval - dt);
        if(!this.isInitRoom) {
            this.curTimer = setTimeout(this.rTimer.bind(this),this.nextTime);
        }
        //console.log("refreshTime");
        this.isInitRoom = false;
    }

    changeTime() {
        this.inVoteTimer.initTime();
        //this.skipCount = 0;
        let p = this.inGameTimer.returnPayload();
        console.log("time refresh - changeTime");
        this.broadcast(JSON.stringify({type:"TIME_REFRESH",payload:p}));
    }

    voteTimer() {
        let dt = Date.now() - this.expected;
        if(this.inVoteTimer.timeRefresh(this.socketList)) {
            if(!this.voteEnd()) {
                this.voteTimeEnd();
                console.log("changeTime - voteTimer");
            }
            return;
        }

        this.expected += this.interval;

        this.nextTime = Math.max(0,this.interval - dt);
        this.curTimer = setTimeout(this.voteTimer.bind(this),this.nextTime);
    }

    addSocket(socket,user) {
        this.socketList.push(socket);
        this.userList[user.socketId] = user;
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

    broadcast(msg,isEnd = false) {
        this.socketList.forEach(soc => {
            if(soc.readyState !== WebSocket.OPEN) return;

            if(isEnd) soc.state = SocketState.IN_ROOM;
            soc.send(msg);
        });
    }

}

module.exports = Room;