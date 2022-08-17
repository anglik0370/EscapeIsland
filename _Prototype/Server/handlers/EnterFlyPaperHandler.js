const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"ENTER_FLY_PAPER",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;
        
        room.broadcast(JSON.stringify({type:"ENTER_FLY_PAPER",payload:JSON.stringify(data)}));
    }
}