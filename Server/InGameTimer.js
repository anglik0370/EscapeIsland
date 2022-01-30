class InGameTimer {
    constructor() {
        this.timeToNextSlot = 60;
        this.curTime = 60;
        this.isLightTime = true;
        this.sec = 1;
        this.day = 1;
    }

    timeRefresh(socketList) {
        this.curTime -= this.sec;

        if(this.curTime <= 0) {
            if(!this.isLightTime) {
                this.day++;
            }
            this.curTime = this.timeToNextSlot;
            this.isLightTime = !this.isLightTime;
    
            socketList.forEach(soc => {
                soc.send(JSON.stringify({type:"TIME_REFRESH",payload:JSON.stringify({day:this.day,isLightTime:this.isLightTime})}))
            });
        }
        
    }
}

module.exports = InGameTimer;