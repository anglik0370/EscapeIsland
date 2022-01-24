class Room {
    constructor(roomName,roomNum,curUserNum,userNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.playing = playing;

        this.socketList = [];
    }

    addSocket(socket) {
        this.socketList.push(socket);
    }

    removesocket(rSocketIdx) {
        let idx = this.socketList.findIndex(soc => soc.id ==rSocketIdx);
        this.socketList.splice(idx);
    }
}

module.exports = Room;