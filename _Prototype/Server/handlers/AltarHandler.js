const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"ALTAR",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;
        
        room.altar(socket,data);
    }
}