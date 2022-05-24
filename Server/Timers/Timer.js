class Timer {
    constructor(room,maxCoolTime,callback) {
        this.maxTime = maxCoolTime;
        this.remainTime = maxCoolTime;

        this.sec = 1;

        this.curTimer = undefined;

        this.interval = 1000;
        this.nextTime = 0;
        this.expected = Date.now();

        this.room = room;
        this.callback = callback;
    }

    initTimer() {
        this.remainTime = this.maxTime;
    }

    setMaxTime(maxTime) {
        this.maxTime = maxTime;
    }

    timeReferesh() {
        this.remainTime -= this.sec;
        if(this.remainTime <= 0) {
            return true;
        }

        return false;
    }

    startTimer(needInit) {
        if(needInit) {
            this.initTimer();
        }
        this.expected = Date.now() + 1000;
        this.curTimer = setTimeout(this.timer.bind(this),this.interval);
    }

    stopTimer() {
        clearInterval(this.curTimer);
    }

    timer() {
        let dt = Date.now() - this.expected;

        if(this.timeReferesh()) {
            //게임끝
            this.callback();
            return;
        }

        this.expected += this.interval;
        this.nextTime = Math.max(0,this.interval - dt);

        this.curTimer = setTimeout(this.timer.bind(this), this.nextTime);
    }
}

module.exports = Timer;