const {Rooms} = require('../Rooms.js');
const {Users} = require('../Users.js');
const ChatType = require('../Utils/ChatType.js');

module.exports = {
    type:"CHAT",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        let user = Users.getUser(socket);

        if(room === undefined || user === undefined) return;

        if(data.chatType == ChatType.Team) {
            room.teamBroadcast(JSON.stringify({type:"CHAT",
            payload:JSON.stringify(data)}),user.curTeam,true);
            return;
        }

        room.broadcast(JSON.stringify({type:"CHAT",
        payload:JSON.stringify(data)}));
    }
}