const {Rooms} = require("../Rooms.js")

module.exports =  {
    type:"JOIN_ROOM",
    act(socket,data) {
        Rooms.joinRoom(socket,data.roomNum);
    }
}