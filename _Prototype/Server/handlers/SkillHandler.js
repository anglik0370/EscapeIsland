const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SKILL",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        if(data.skillName == "마술") {
            data.itemId = room.userList[data.targetId].getItem();
        }

        room.broadcast(JSON.stringify({type:"SKILL",
        payload:JSON.stringify(data)}));
    }
}