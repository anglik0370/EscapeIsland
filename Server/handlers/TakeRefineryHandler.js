const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"TAKE_REFINERY",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.broadcast(JSON.stringify({type:"TAKE_REFINERY",payload:data.refineryId}));
    }
}