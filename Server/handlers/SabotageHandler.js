const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"SABOTAGE",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        data.userDataList = Object.values(room.userList);
        room.broadcast(JSON.stringify({type:"SABOTAGE",payload:JSON.stringify(data)}));

        if(data.sabotageName === "Arson") {
            room.arsonTimer.startTimer(true);
        }
    }
}