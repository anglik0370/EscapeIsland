const team = require("./Utils/Team.js");

class Area {
    constructor(areaState) {

        this.areaState = areaState;

        this.redTeamUserList = [];
        this.blueTeamUserList = [];

        this.blueGauge = 0.0;
        this.redGauge = 0.0;

        this.occupyTeam = team.NONE;

        //Timer
        this.curTimer = undefined;

        this.occupyTime = 30000;
        this.interval = 200;
        this.nextTime = 0;
        this.expected = Date.now();
    }


    getPayload() {
        return {area:this.areaState, blueGauge:this.blueGauge, redGauge:this.redGauge};
    }

    canMission(user) {
        return (this.occupyTeam != team.NONE&& 
            (user.curTeam == team.RED ? this.blueTeamUserList.length <= this.redTeamUserList.length 
            : this.blueTeamUserList.length >= this.redTeamUserList.length))
                 || this.occupyTeam === user.curTeam;
    }

    startTimer() {
        this.occupyTeam = team.NONE;
        this.blueGauge = this.redGauge = 0.0;

        this.expected = Date.now() + 1000;
        this.curTimer = setTimeout(this.timer.bind(this),this.interval);
    }

    initTimer() {
        this.occupyTeam = team.NONE;
        this.blueGauge = this.redGauge = 0.0;

        clearInterval(this.curTimer);
        this.curTimer = null;
    }

    timer() {
        let dt = Date.now() - this.expected;

        this.blueGauge += (this.blueTeamUserList.length * 0.02);
        this.redGauge += (this.redTeamUserList.length * 0.02);

        if(this.blueGauge >= 1 || this.redGauge >= 1) {
            this.occupy();
            return;
        }

        this.expected += this.interval;
        this.nextTime = Math.max(0,this.interval - dt);

        this.curTimer = setTimeout(this.timer.bind(this), this.nextTime);
    }

    occupy() {
        this.curTimer = null;
        this.occupyTeam = this.blueGauge >= 1 ? team.BLUE : team.RED;

        setTimeout(this.startTimer.bind(this),this.occupyTime);
    }

    addUserList(user, isBlue) {
        if(isBlue && !this.blueTeamUserList.includes(user)) {
            this.blueTeamUserList.push(user);
        }
        else if(!isBlue && !this.redTeamUserList.includes(user)) {
            this.redTeamUserList.push(user);
        }
    }

    removeUserList(user,isBlue) {
        if(isBlue) {
            this.blueTeamUserList = this.blueTeamUserList.filter(x => x !== user)
        }
        else {
            this.redTeamUserList = this.redTeamUserList.filter(x => x !== user);
        }
    }
}   

module.exports = Area;