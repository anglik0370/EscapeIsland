const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SPAWNER_OPEN",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.setSpawnerData(socket,data);
    }
}