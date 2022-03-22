const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"START_REFINERY",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        if(room === undefined) return;
        
        room.broadcast(JSON.stringify({type:"START_REFINERY",payload:JSON.stringify({refineryId:data.refineryId,itemSOId:data.itemSOId})}))
    }
}