const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"ENTER_TRAP",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;
        
        room.broadcast(JSON.stringify({type:"ENTER_TRAP",payload:JSON.stringify(data)}));
    }
}