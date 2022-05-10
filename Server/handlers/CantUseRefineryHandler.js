const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"CANT_USE_REFINERY",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.broadcast(JSON.stringify({type:"CANT_USE_REFINERY",
            payload:JSON.stringify(data)}));
    }
}