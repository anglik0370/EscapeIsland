const Timer = require('./Timer.js');

class InGameTimer extends Timer{
    constructor(room,maxCoolTime,callback) {
        super(room,maxCoolTime,callback);

        this.isLightTime = true;
        this.day = 1;
    }

    returnPayload() {
        return JSON.stringify({day:this.day,isLightTime:this.isLightTime});
    }

    stopTimer(isInit = false) {
        clearInterval(this.curTimer);

        if(isInit) {
            this.initTimer();
            this.isLightTime = true;
            this.day = 1;
        }
    }

    timer() {
        let dt = Date.now() - this.expected;

        if(this.timeReferesh()) {
            this.callback();

            if(!this.isLightTime) {
                this.day++;
            }
            this.remainTime = this.maxTime;
            this.isLightTime = !this.isLightTime;

            this.room.broadcast(JSON.stringify({type:"TIME_REFRESH",payload:this.returnPayload()}));
        }

        this.expected += this.interval;
        this.nextTime = Math.max(0,this.interval - dt);

        this.curTimer = setTimeout(this.timer.bind(this), this.nextTime);
    }
}

module.exports = InGameTimer;