const SocketState = require('./SocketState.js');

class Users {
    constructor() {
        this.userList = {};
        this.connectedSocket = {};
        this.socketIdx = 1;
    }

    connect(socket,wsService) {
        socket.state = SocketState.IN_LOGIN;
        socket.id = this.socketIdx;
        this.connectedSocket[this.socketIdx] = socket;
        this.socketIdx++;
        socket.room = -1;
        socket.server = wsService;
    }
}

module.exports = {
    Users: new Users()
}