const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"EXTINGUISH",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.broadcast(JSON.stringify({type:"EXTINGUISH",payload:null}));
        room.arsonTimer.stopTimer();
    }
}