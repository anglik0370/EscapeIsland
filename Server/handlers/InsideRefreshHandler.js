const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"INSIDE_REFRESH",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        if(room === undefined) return;

        room.insideRefresh(socket,data.isInside);
    }
}