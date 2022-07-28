const Vector2 = require("./Utils/Vector2");
const team = require('./Utils/Team.js');
const AreaState = require("./Utils/AreaState");

class Player {
    constructor() {
        this.name = "";

        this.position = Vector2.zero;

        this.socketId = -1;
        this.charId = 0;
        this.roomNum = -1;
        this.areaState = AreaState.None;

        this.master = false;
        this.ready = false;

        this.curTeam = team.NONE;

        this.voiceData = [];
    }

    setVoiceData(newData) {
        if(newData === undefined || newData === null || newData.length <= 0) return;

        this.voiceData = [
            ...this.voiceData,
            ...newData
        ];
    }

    initLoginData(socketId,name,roomNum) {
        this.socketId = socketId;
        this.name = name;
        this.roomNum = roomNum;
    }

    initExitData() {
        this.roomNum = 0;
        this.charId = 0;
        this.master = false; 
        this.areaState = 0;
        this.ready = false;

        this.curTeam = team.NONE;
    }
}

module.exports = Player;
