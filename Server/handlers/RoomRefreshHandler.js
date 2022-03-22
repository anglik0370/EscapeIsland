const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"ROOM_REFRESH_REQ",
    act(socket,data) {
        Rooms.refreshRoom(socket);
    }
}