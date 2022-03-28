const {Rooms} = require('../Rooms.js');
const SetSpawnPoint = require('../Utils/GameSpawnHandler.js');

module.exports = {
    type:"EMERGENCY",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;
    
        let keys = Object.keys(room.userList);
    
        let posList = SetSpawnPoint(keys.length);
    
        for(let i = 0; i < keys.length; i++) {
            room.userList[keys[i]].position = posList[i];
        }
    
        room.startVoteTimer();
        room.isEnd = false;
    
        let dataList = Object.values(room.userList);
    
        room.broadcast(JSON.stringify({type:"VOTE_TIME",payload:JSON.stringify({dataList,type:0})}));
    }
}