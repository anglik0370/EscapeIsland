const {Rooms} = require('../Rooms.js');
const {Users} = require('../Users.js');

module.exports = {
    type:"CHANGE_TEAM",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        let user = Users.getUser(socket);

        if(room === undefined || user === undefined || user === null) return;
        user.curTeam = data.team;
        
        room.broadcast(JSON.stringify({type:"CHANGE_TEAM",
            payload:JSON.stringify(user)}));
    }
}