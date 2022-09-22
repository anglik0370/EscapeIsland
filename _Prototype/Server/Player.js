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
        this.area = AreaState.None;

        this.master = false;
        this.ready = false;
        this.testClient = false;

        this.curTeam = team.NONE;

        this.voiceData = [];
        this.itemList = [];
    }

    getItem() {
        if(this.itemList.length <= 0) return -1;

        let idx = Math.floor(Math.random() * this.itemList.length);
        let item = this.itemList[idx];
        
        this.itemList.splice(idx,1);

        return item;
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
        this.area = 0;
        this.ready = false;

        this.curTeam = team.NONE;
    }
}

module.exports = Player;
