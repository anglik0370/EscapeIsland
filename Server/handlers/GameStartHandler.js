const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"GameStart",
    act(socket,data) {
        Rooms.getRoom(data.roomNum).gameStart(socket);
    }
}