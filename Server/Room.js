const InGameTimer = require('./InGameTimer.js');
const InVoteTimer = require('./InVoteTimer.js');
const SetSpawnPoint = require('./GameSpawnHandler.js');

class Room {
    constructor(roomName,roomNum,curUserNum,userNum,kidnapperNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.kidnapperNum = kidnapperNum;
        this.playing = playing;

        this.inGameTimer = new InGameTimer();
        this.inVoteTImer = new InVoteTimer();
        this.curTimer = undefined;
        
        this.interval = 1000;
        this.nextTime = 0;
        this.expected = Date.now();

        this.socketList = [];
        this.userList = {};

        this.isEnd = true;
    }

    startTimer() {
        this.expected = Date.now() + 1000; //현재시간 + 1초
        this.curTimer = setTimeout(this.rTimer.bind(this),this.interval);
    }

    startVoteTimer() {
        //this.inVoteTImer.initTime();
        this.stopTimer();
        this.curTimer = setTimeout(this.voteTimer.bind(this),this.interval,false);
    }

    stopTimer() {
        clearTimeout(this.curTimer);
    }

    rTimer() {
        let dt = Date.now() - this.expected; //현재 시간 - 시작시간

        if(this.inGameTimer.timeRefresh(this.socketList)) {
            let keys = Object.keys(this.userList);
            let posList = SetSpawnPoint(keys.length);

            for(let i = 0; i< keys.length; i++) {
                this.userList[keys[i]].position = posList[i];
            }

            let dataList = Object.values(this.userList);

            this.socketList.forEach(soc => {
                soc.send(JSON.stringify({type:"VOTE_TIME",payload:JSON.stringify({dataList})}));
            });
            this.expected = Date.now() + 1000;
            this.curTimer = setTimeout(this.voteTimer.bind(this),this.interval,true);
            return;
        }

        this.expected += this.interval;

        this.nextTime = Math.max(0,this.interval - dt);
        this.curTimer = setTimeout(this.rTimer.bind(this),this.nextTime);
    }

    changeTime() {
        this.inVoteTImer.initTime();
        let p = this.inGameTimer.returnPayload();

            this.socketList.forEach(soc => {
                soc.send(JSON.stringify({type:"TIME_REFRESH",payload:p}));
            });
            
            let keys = Object.keys(this.userList);

            for(let i = 0; i < keys.length; i++) {
                this.userList[keys[i]].voteNum = 0;
                this.userList[keys[i]].voteComplete = false;
            }
            
            this.startTimer();
    }

    voteTimer(isEnd) {
        let dt = Date.now() - this.expected;

        if(this.inVoteTImer.timeRefresh()) {
            let dummy = 0;
            let targetSocIdArr = [];

            let comRoomKeys = Object.keys(this.userList);

            for(let i = 0; i < comRoomKeys.length; i++) {
                if(dummy != 0 && this.userList[comRoomKeys[i]].voteNum == dummy) {
                    targetSocIdArr.push(this.userList[comRoomKeys[i]].socketId);
                }
                else if(this.userList[comRoomKeys[i]].voteNum > dummy) {
                    dummy = this.userList[comRoomKeys[i]].voteNum;
                    targetSocIdArr.length = 0;
                    targetSocIdArr.push(this.userList[comRoomKeys[i]].socketId);
                }

                this.userList[comRoomKeys[i]].voteNum = 0;
                this.userList[comRoomKeys[i]].voteComplete = false;
            }
            
            
            if(targetSocIdArr.length == 1) {
                this.socketList.forEach(soc => {
                    soc.send(JSON.stringify({type:"VOTE_DIE",payload:targetSocIdArr[0]}));
                });
                this.userList[targetSocIdArr[0]].isDie = true;
            }

            if(isEnd) {
                this.changeTime();
            }
            else {
                this.socketList.forEach(soc => {
                    soc.send(JSON.stringify({type:"VOTE_TIME_END",payload:""}));
                });

                this.startTimer();
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

    deleteRoom() {
        clearTimeout(this.curTimer);
    }

    

    returnData() {
        let data = {name:this.roomName,roomNum:this.roomNum,curUserNum:this.curUserNum,userNum:this.userNum,kidnapperNum:this.kidnapperNum,playing:this.playing};
        return data;
    }

}

module.exports = Room;