const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SKILL",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.broadcast(JSON.stringify({type:"SKILL",
        payload:JSON.stringify(data)}));
    }
}