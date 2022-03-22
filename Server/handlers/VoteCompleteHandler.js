const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"VOTE_COMPLETE",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;
    
        if(data.voteTargetId === -1){
            room.skipCount++;
        }
        else {
            room.userList[data.voteTargetId].voteNum++;
        }
    
        room.userList[data.voterId].voteComplete = true;
    
        room.broadcast(JSON.stringify({type:"VOTE_COMPLETE",payload:JSON.stringify({voterId:data.voterId,voteTargetId:data.voteTargetId})}));
    
        room.voteEnd();
    }
}