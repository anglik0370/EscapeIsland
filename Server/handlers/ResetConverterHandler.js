const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"RESET_CONVERTER",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.broadcast(JSON.stringify({type:"RESET_CONVERTER",payload:data.refineryId}));
    }
}