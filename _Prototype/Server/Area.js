const team = require("./Utils/Team.js");

class Area {
    constructor(areaState,areaName) {

        this.areaState = areaState;
        this.areaName = areaName;

        this.redTeamUserLength = 0.0;
        this.blueTeamUserLength = 0.0;

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
        return {area:this.areaState,areaName:this.areaName, blueGauge:this.blueGauge, redGauge:this.redGauge, occupyTeam : this.occupyTeam};
    }

    canMission(user) {
        return (this.occupyTeam == team.NONE&& 
            (user.curTeam == team.RED ? this.blueTeamUserLength <= this.redTeamUserLength
            : this.blueTeamUserLength >= this.redTeamUserLength))
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

        clearTimeout(this.curTimer);
        this.curTimer = null;
    }

    timer() {
        let dt = Date.now() - this.expected;

        this.blueGauge += (this.blueTeamUserLength * 0.02);
        this.redGauge += (this.redTeamUserLength * 0.02);

        if(this.blueGauge >= 1 || this.redGauge >= 1) {
            this.occupy();
            return;
        }

        this.expected += this.interval;
        this.nextTime = Math.max(0,this.interval - dt);

        this.curTimer = setTimeout(this.timer.bind(this), this.nextTime);
    }

    occupy() {
        this.occupyTeam = this.blueGauge >= this.redGauge ? team.BLUE : team.RED;

        clearTimeout(this.curTimer);
        this.curTimer = setTimeout(this.startTimer.bind(this),this.occupyTime);
    }

    addUserList(isBlue,isWonsong = false) {
        if(isBlue) {
            let add = this.blueTeamUserLength + (isWonsong ? 1.5 : 1);
            this.blueTeamUserLength = add;
        }
        else if(!isBlue) {
            let add = this.redTeamUserLength + (isWonsong ? 1.5 : 1);
            this.redTeamUserLength = add;
        }
    }

    removeUserList(isBlue,isWonsong = false) {
        if(isBlue) {
            let remove = this.blueTeamUserLength - (isWonsong ? 1.5 : 1);
            this.blueTeamUserLength = remove;
        }
        else {
            let remove = this.redTeamUserLength - (isWonsong ? 1.5 : 1);
            this.redTeamUserLength = remove;
        }
    }
}   

module.exports = Area;