const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"CHARACTER_CHANGE",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.changeCharacter(data.beforeCharacterId,data.characterId);

        room.socketList.forEach(soc => {
            if(soc.id == socket.id) return;
            soc.send(JSON.stringify({type:"CHARACTER_CHANGE",
            payload:JSON.stringify(data)}));
        });

    }
}