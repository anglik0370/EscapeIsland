const SocketState = require('./Utils/SocketState.js');

class Users {
    constructor() {
        this.userList = {};
        this.connectedSocket = {};
        this.socketIdx = 1;

        this.isTestServer = false;
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