const Timer = require('./Timer.js');

class VoteTimer extends Timer{
    constructor(room,maxCoolTime,discussionTime,callback) {
        super(room,maxCoolTime,callback);

        this.discussionTime = discussionTime;

        this.integrationTime = this.discussionTime + this.remainTime;
    }

    timeReferesh() {
        this.IntegrationTime -= this.sec;
        if(this.IntegrationTime <= 0) {
            return true;
        }

        return false;
    }
}

module.exports = VoteTimer;