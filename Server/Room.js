class Room {
    constructor(roomName,roomNum,curUserNum,userNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.playing = playing;

        this.socketList = [];
        this.userList = {};
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
        let data = {};
        data.name = this.roomName;
        data.roomNum = this.roomNum; 
        data.curUserNum = this.curUserNum;
        data.userNum = this.userNum; 
        data.playing = this.playing; 
        return data;
    }

}

module.exports = Room;