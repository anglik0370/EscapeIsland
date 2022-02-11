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
    }

    startTimer() {
        this.expected = Date.now() + 1000; //현재시간 + 1초
        this.curTimer = setTimeout(this.rTimer.bind(this),this.interval);
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
            this.curTimer = setTimeout(this.voteTimer.bind(this),this.interval);
            console.log("vote time start");
            return;
        }

        this.expected += this.interval;

        this.nextTime = Math.max(0,this.interval - dt);
        this.curTimer = setTimeout(this.rTimer.bind(this),this.nextTime);
    }

    voteTimer() {
        let dt = Date.now() - this.expected;

        if(this.inVoteTImer.timeRefresh()) {
            let p = this.inGameTimer.returnPayload();

            this.socketList.forEach(soc => {
                soc.send(JSON.stringify({type:"TIME_REFRESH",payload:p}));
            });
            console.log("vote time end");
            this.startTimer();
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