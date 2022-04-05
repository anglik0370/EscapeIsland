const Vector2 = require("./Utils/Vector2");

class Player {
    constructor() {
        this.name = "";

        this.position = Vector2.zero;

        this.voteNum = 0;
        this.socketId = -1;
        this.roomNum = -1;

        this.isImposter = false;
        this.master = false;
        this.isDie = false;
        this.isInside = false;
        this.voteComplete = false;
    }

    initLoginData(socketId,name,roomNum) {
        this.socketId = socketId;
        this.name = name;
        this.roomNum = roomNum;
    }

    initExitData() {
        this.roomNum = 0;
        this.master = false; 
        this.isImposter = false;
        this.isDie = false;
        this.voteNum = 0;
        this.voteComplete = false;
        this.isInside = false;
    }
}

module.exports = Player;
