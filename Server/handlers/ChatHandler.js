const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"CHAT",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.broadcast(JSON.stringify({type:"CHAT",payload:JSON.stringify({data})}));
    }
}