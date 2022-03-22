const {Rooms}= require("../Rooms.js")



module.exports =  {
    type:"CREATE_ROOM",
    act(socket,data) {
        Rooms.createRoom(socket,data);
    }
}