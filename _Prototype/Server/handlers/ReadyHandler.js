const {Rooms} = require('../Rooms.js');
const {Users} = require('../Users.js');

module.exports = {
    type:"READY",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        let user = Users.getUser(socket);

        if(room === undefined) return;
        if(user === undefined) return;

        if(user.ready) return;
        
        user.ready = true;
        room.broadcast(JSON.stringify({type:"READY",payload:JSON.stringify({socketId:socket.id})}));
    }
}