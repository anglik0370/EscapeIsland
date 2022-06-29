const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SKILL_JOSHUA",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        if(room === undefined) return;

        room.socketList.forEach(soc => {
            if(data.targetIdList.includes(soc.id)) {
                soc.send(JSON.stringify({type:"SKILL_JOSHUA",payload:null}));
            }
        });
    }
}