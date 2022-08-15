const {Rooms} = require('../Rooms.js');
const team = require('../Utils/Team.js');

module.exports = {
    type:"STORAGE_DROP",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        
        if(room === undefined) return;
        
        room.storageDrop(data);
    }
}