class ArsonTimer {
    constructor() {
        this.maxTime = 40;
        this.ramainTime = 40;

        this.sec = 1;

        this.curTimer = undefined;

        this.interval = 1000;
        this.nextTime = 0;
        this.expected = Date.now();
    }

    initTimer() {
        this.ramainTime = this.maxTime;
    }

    timeReferesh() {
        this.ramainTime -= this.sec;

        if(this.ramainTime <= 0) {
            return true;
        }

        return false;
    }

    startArsonTimer() {
        this.initTimer();
        this.expected = Date.now() + 1000;
        this.curTimer = setTimeout(thiis.arsonTimer.bind(this),this.interval);
    }

    arsonTimer() {
        let dt = Date.now() - this.expected;

        if(this.timeReferesh()) {
            //게임끝
            return;
        }

        this.expected += this.interval;
        this.nextTime = Math.max(0,this.interval - dt);

        this.curTimer = setTimeout(this.arsonTimer.bind(this), this.nextTime);
    }
}

module.exports = ArsonTimer;