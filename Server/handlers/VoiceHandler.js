const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"VOICE",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        room.addVoiceData(socket.id,data.voiceData);
    }
}