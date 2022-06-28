const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SKILL_CHERRY",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.socketList.forEach(soc => {
            if(soc.id in data.targetIdList) {
                soc.send(JSON.stringify({type:"SKILL_CHERRY",payload:JSON.stringify(data)}));
            }
        });
    }
}