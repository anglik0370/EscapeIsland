class InGameTimer {
    constructor() {
        this.timeToNextSlot = 60;
        this.curTime = 20;
        this.isLightTime = true;
        this.sec = 1;
        this.day = 1;
    }

    returnPayload() {
        return JSON.stringify({day:this.day,isLightTime:this.isLightTime});
    }

    timeRefresh(socketList) {
        this.curTime -= this.sec;

        if(this.curTime <= 0) {
            if(!this.isLightTime) {
                this.day++;
            }
            this.curTime = this.timeToNextSlot;
            this.isLightTime = !this.isLightTime;
            
            if(!this.isLightTime) {
                return true;
            }
            else {
                socketList.forEach(soc => {
                    soc.send(JSON.stringify({type:"TIME_REFRESH",payload:JSON.stringify({day:this.day,isLightTime:this.isLightTime})}))
                });

                return false;
            }
    
            
        }
        else {
            return false;
        }
        
    }
}

module.exports = InGameTimer;