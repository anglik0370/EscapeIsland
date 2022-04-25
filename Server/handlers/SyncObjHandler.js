const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SYNC_OBJ",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;
        room.socketList.forEach(soc => {
            soc.send(JSON.stringify({type:"SYNC_OBJ",
            payload:JSON.stringify(data)}));
        });
    }
}