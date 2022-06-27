const {Rooms} = require('../Rooms.js');
const team = require('../Utils/Team.js');

module.exports = {
    type:"SABOTAGE",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        data.userDataList = Object.values(room.userList);
        room.broadcast(JSON.stringify({type:"SABOTAGE",payload:JSON.stringify(data)}));

        let isBlue = data.team == team.BLUE;

        if(data.sabotageName === "방화") {
            room.arsonTimer.startTimer(true,isBlue ? team.RED : team.BLUE);
        }
    }
}