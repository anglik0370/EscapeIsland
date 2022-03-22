const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"KILL",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        let targetSocId = data.targetSocketId;

        if(room.userList[targetSocId] !== undefined) {
            room.userList[targetSocId].isDie = true;
        }

        Rooms.roomBroadcast(socket.room,"KILL");
        
        room.kidnapperWinCheck();
    }
}