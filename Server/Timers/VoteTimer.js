const Timer = require('./Timer.js');

class VoteTimer extends Timer{
    constructor(room,maxCoolTime,discussionTime,callback) {
        super(room,maxCoolTime,callback);

        this.discussionTime = discussionTime;
    }
}

module.exports = VoteTimer;