class InVoteTimer {
    constructor() {
        this.timeToNextSlot = 180;
        this.curTime = 10;
        this.sec = 1;
    }

    initTime() {
        this.curTime = this.timeToNextSlot;
    }

    timeRefresh(socketList) {
        this.curTime -= this.sec;

        socketList.forEach(soc => {
            soc.send(JSON.stringify({type:"TIMER",payload:this.curTime}));
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