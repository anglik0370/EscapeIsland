const {Rooms} = require('../Rooms.js');
const {Users} = require('../Users.js');

module.exports = {
    type:"CHARACTER_CHANGE",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        let user = Users.userList[socket.id];
        if(room === undefined || user === undefined) return;

        room.changeCharacter(data.team, data.beforeCharacterId,data.characterId);
        user.charId = data.characterId;
        room.socketList.forEach(soc => {
            if(soc.id == socket.id) return;
            soc.send(JSON.stringify({type:"CHARACTER_CHANGE",
            payload:JSON.stringify(data)}));
        });

    }
}