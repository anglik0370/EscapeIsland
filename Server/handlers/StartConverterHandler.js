const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"START_CONVERTER",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        if(room === undefined) return;
        
        room.broadcast(JSON.stringify({type:"START_CONVERTER",payload:JSON.stringify({refineryId:data.refineryId,itemSOId:data.itemSOId})}))
    }
}