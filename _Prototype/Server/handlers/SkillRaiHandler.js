const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SKILL_RAI",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        if(room === undefined) return;

        room.socketList.forEach(soc => {
            if(data.targetId == soc.id) {
                soc.send(JSON.stringify({type:"SKILL_RAI",payload:null}));
            }
        });
    }
}