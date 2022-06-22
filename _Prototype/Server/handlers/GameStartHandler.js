const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"GameStart",
    act(socket,data) {
        let room = Rooms.getRoom(data.roomNum);

        if(room === undefined) return;

        room.gameStart(socket);
    }
}