const {Rooms} = require('../Rooms.js');
const {Users} = require('../Users.js');

module.exports = {
    type:"NOT_LERP_MOVE",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        let user = Users.getUser(socket);
        user.position = data.pos;
        room.broadcast(JSON.stringify({type:"NOT_LERP_MOVE",payload:JSON.stringify(data)}));
    }
}