const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"REMOVE_ALL_ROOM",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.removeAllRoom();
    }
}