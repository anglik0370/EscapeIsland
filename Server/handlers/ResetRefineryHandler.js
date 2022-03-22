const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"RESET_REFINERY",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.broadcast(JSON.stringify({type:"RESET_REFINERY",payload:data.refineryId}));
    }
}