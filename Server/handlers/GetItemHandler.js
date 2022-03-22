const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"GET_ITEM",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.broadcast(JSON.stringify({type:"GET_ITEM",payload:data.spawnerId}))
    }
}