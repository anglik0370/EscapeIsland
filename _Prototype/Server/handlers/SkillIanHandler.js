const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SKILL_IAN",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        let user = room.userList[socket.id];

        if(room === undefined || user === undefined) return;

        room.teamBroadcast(JSON.stringify({type:"SKILL_IAN",
        payload:null}),user.curTeam,data.isSame);
    }
}