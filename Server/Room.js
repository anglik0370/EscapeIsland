class Room {
    constructor(roomName,roomNum,curUserNum,userNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.playing = playing;

        this.socketList = [];
        this.userList = [];
    }

    addSocket(socket,user) {
        this.socketList.push(socket);
        this.userList.push(user);
    }

    removeSocket(rSocketIdx) {
        let idx = this.socketList.findIndex(soc => soc.id == rSocketIdx);
        this.socketList.splice(idx);
        this.userList.splice(idx);
    }

}

module.exports = Room;