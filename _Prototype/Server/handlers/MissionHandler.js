const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"MISSION",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.teamBroadcast(JSON.stringify({type:"MISSION",payload:JSON.stringify(data)})
        ,data.team, true);
    }
}