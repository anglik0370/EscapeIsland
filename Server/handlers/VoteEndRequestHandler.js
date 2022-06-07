const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"VOTE_END_REQ",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.voteResult();
    }
}