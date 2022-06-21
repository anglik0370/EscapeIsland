const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"ROOM_DELETE_REQ",
    act(socket,data) {

        Rooms.removeAllRoom(socket);
    }
}