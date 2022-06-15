const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"WIN",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.endGameHandle(data.goc);
    }
};