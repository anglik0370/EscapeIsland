const {Rooms} = require('./Rooms.js');

class InGameTimer {
    constructor() {
        this.timeToNextSlot = 120;
        this.curTime = 120;
        this.isLightTime = true;
        this.sec = 1;
        this.day = 1;

        this.isEndGame = false;
    }

    setTimeToNextSlot(time) {
        this.timeToNextSlot = time;
        this.curTime = this.timeToNextSlot;
    }

    returnPayload() {
        return JSON.stringify({day:this.day,isLightTime:this.isLightTime});
    }

    timeRefresh(socketList) {
        this.curTime -= this.sec;

        // socketList.forEach(soc => {
        //     soc.send(JSON.stringify({type:"TIMER",payload:JSON.stringify({curTime:this.curTime,isInGameTimer:true})}));
        // });

        if(this.curTime <= 0) {
            if(this.isEndGame) {
                let key = Object.keys(socketList);
                let room = Rooms.getRoom(this.socketList[key[i]].room);

                if(room === undefined) {
                    console.log("undefined");
                    return;
                }

                let keys = Object.keys(room.userList);
                let posList = SetSpawnPoint(keys.length);
        
                for(let i = 0; i < keys.length; i++) {
                    room.userList[keys[i]].position = posList[i];
                }
        
                let dataList = Object.values(room.userList);

                room.broadcast(JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:2})}),true);
                room.initRoom();
                return;
            }

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