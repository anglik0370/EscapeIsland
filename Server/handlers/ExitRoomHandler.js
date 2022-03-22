const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"EXIT_ROOM",
    act(socket,data) {
        Rooms.exitRoom(socket,data.roomNum);
    }
}