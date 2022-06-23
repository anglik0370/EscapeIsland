const {Rooms} = require('../Rooms.js');
const {Users} = require('../Users.js');

const team = require('../Utils/Team.js');

module.exports = {
    type:"CHANGE_TEAM",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);
        let user = Users.getUser(socket);

        if(room === undefined || user === undefined || user === null) return;
        room.changeCharacter(user.curTeam,user.charId,0);
        user.curTeam = data.team;
        user.charId = 0;
        let redSelectedCharId = room.getSelectedIdList(team.RED);
        let blueSelectedCharId = room.getSelectedIdList(team.BLUE);
        
        room.broadcast(JSON.stringify({type:"CHANGE_TEAM",
            payload:JSON.stringify({user,redSelectedCharId,blueSelectedCharId})}));
    }
}