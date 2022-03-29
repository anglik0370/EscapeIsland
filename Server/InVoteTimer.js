class InVoteTimer {
    constructor() {
        this.timeToNextSlot = 180;
        this.curTime = 180;
        this.sec = 1;
    }

    initTime() {
        this.curTime = this.timeToNextSlot;
    }

    setTimeToNextSlot(time) {
        this.timeToNextSlot = time;
        this.curTime = this.timeToNextSlot;
    }

    timeRefresh(socketList) {
        this.curTime -= this.sec;

        socketList.forEach(soc => {
            soc.send(JSON.stringify({type:"TIMER",payload:JSON.stringify({curTime:this.curTime,isInGameTimer:false})}));
        });

        if(this.curTime <= 0) {
            
            this.curTime = this.timeToNextSlot;
            return true;
        }
        else {
            return false;
        }
        
    }
}

module.exports = InVoteTimer;