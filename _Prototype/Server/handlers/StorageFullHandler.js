const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"STORAGE_FULL",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;


        room.setSpawnPos();
    
        let dataList = Object.values(room.userList);
        room.broadcast(JSON.stringify({type:"WIN",payload:JSON.stringify({dataList,gameOverCase:data.team})}),true);
        room.initRoom();
    }
}