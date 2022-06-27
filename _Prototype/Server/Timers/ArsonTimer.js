const Timer = require('./Timer.js');
const team = require('../Utils/Team.js');

class ArsonTimer extends Timer{
    constructor(room,maxCoolTime,callback) {
        super(room,maxCoolTime,callback);

        this.team = team.NONE;
    }

    returnPayload() {
        return JSON.stringify({day:this.day,isLightTime:this.isLightTime});
    }

    stopTimer(isInit = false) {
        clearInterval(this.curTimer);

        if(isInit) {
            this.initTimer();
            this.team = team.NONE;
        }
    }

    startTimer(needInit,team) {
        if(needInit) {
            this.initTimer();
        }
        this.team = team;
        this.expected = Date.now() + 1000;
        this.curTimer = setTimeout(this.timer.bind(this),this.interval);
    }

    timer() {
        let dt = Date.now() - this.expected;

        if(this.timeReferesh()) {
            this.callback();
            this.initTimer();
        }

        this.expected += this.interval;
        this.nextTime = Math.max(0,this.interval - dt);

        this.curTimer = setTimeout(this.timer.bind(this), this.nextTime);
    }
}

module.exports = ArsonTimer;