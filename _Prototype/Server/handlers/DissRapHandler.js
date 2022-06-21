const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"DISS_RAP",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        let user = room.userList[socket.id];

        if(user === undefined) return;

        let team = user.curTeam;

        room.teamBroadcast(JSON.stringify({type:"DISS_RAP",
        payload:null}),team,false);
    }
};