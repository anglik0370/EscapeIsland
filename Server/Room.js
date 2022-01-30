const InGameTimer = require('./InGameTimer.js');

class Room {
    constructor(roomName,roomNum,curUserNum,userNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.playing = playing;

        this.inGameTimer = new InGameTimer();
        this.interval = 1000;
        this.nextTime = 0;
        this.expected = Date.now();

        this.socketList = [];
        this.userList = {};
    }

    startTimer() {
        this.expected = Date.now() + 1000; //현재시간 + 1초
        setTimeout(this.rTimer.bind(this),this.interval);
    }

    rTimer() {
        let dt = Date.now() - this.expected; //현재 시간 - 시작시간

        this.inGameTimer.timeRefresh(this.socketList);

        this.expected += this.interval;

        this.nextTime = Math.max(0,this.interval - dt);
        setTimeout(this.rTimer.bind(this),this.nextTime);
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
        let data = {name:this.roomName,roomNum:this.roomNum,curUserNum:this.curUserNum,userNum:this.userNum,playing:this.playing};
        return data;
    }

}

module.exports = Room;