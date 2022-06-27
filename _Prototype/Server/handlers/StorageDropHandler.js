const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"STORAGE_DROP",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        
        if(room === undefined) return;
        
        room.storageItemList.push(data.itemSOId);
        room.broadcast(JSON.stringify({type:"STORAGE_DROP",payload:JSON.stringify(data)}));
    }
}