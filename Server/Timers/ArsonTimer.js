class ArsonTimer {
    constructor(room) {
        this.maxTime = 40;
        this.ramainTime = 40;

        this.sec = 1;

        this.curTimer = undefined;

        this.interval = 1000;
        this.nextTime = 0;
        this.expected = Date.now();

        this.room = room;
    }

    initTimer() {
        this.ramainTime = this.maxTime;
    }

    timeReferesh() {
        this.ramainTime -= this.sec;
        console.log(this.ramainTime);
        if(this.ramainTime <= 0) {
            return true;
        }

        return false;
    }

    startArsonTimer() {
        this.initTimer();
        this.expected = Date.now() + 1000;
        this.curTimer = setTimeout(this.arsonTimer.bind(this),this.interval);
    }

    stopTimer() {
        clearInterval(this.curTimer);
    }

    arsonTimer() {
        let dt = Date.now() - this.expected;

        if(this.timeReferesh()) {
            //게임끝
            this.room.sendKidnapperWin(0);
            return;
        }

        this.expected += this.interval;
        this.nextTime = Math.max(0,this.interval - dt);

        this.curTimer = setTimeout(this.arsonTimer.bind(this), this.nextTime);
    }
}

module.exports = ArsonTimer;