const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SABOTAGE",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        if(data.isShareCoolTime) {
            room.broadcast(JSON.stringify({type:"SABOTAGE",payload:JSON.stringify(data)}));
        }
        else {
            socket.send(JSON.stringify({type:"SABOTAGE",payload:JSON.stringify(data)}));
        }
    }
}