const {Rooms} = require('../Rooms.js');

const sendError = require('../Utils/SendError.js');

module.exports = {
    type:"FIND_ROOM",
    act(socket,data) {
        console.log(data);
        let room = Rooms.findRoom(data.name);

        if(room === undefined) {
            sendError("존재하지 않는 방입니다.",socket);
            return;
        }

        Rooms.joinRoom(socket,room.roomNum);
    }
}