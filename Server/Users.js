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

    disconnect(socket, wsService,Rooms) {
        let roomNum = socket.room; //현재 소켓의 룸 idx를 받아옴

        //현재 socket이 룸에 들어가 있다면
        if(socket.room > 0 && socket.state === SocketState.IN_ROOM) {
            Rooms.exit(socket,roomNum); //방 나가기
            wsService.clients.forEach(soc => {
                if(soc.room === roomNum) { //소켓이 해제된 유저가 있었던 방에 있다면
                    //refreshUser(soc,roomNum); 
                    Rooms.roomBroadcast(roomNum);
                }
                if(soc.state === SocketState.IN_LOBBY) {
                    Rooms.refreshRoom(soc);
                }
            });
        }
        //현재 socket의 state가 IN_PLAYING일때
        if(socket.state === SocketState.IN_PLAYING) {
            if(socket.room > 0) {
                Rooms.exit(socket,roomNum);
            }
            wsService.clients.forEach(soc=>{
                if(soc.state === SocketState.IN_LOBBY) {
                    Rooms.refreshRoom(soc);
                }
                if(soc.room === roomNum) {
                    Rooms.roomBroadcast(roomNum);
                }
            })

        }
        delete this.connectedSocket[socket.id];
        delete this.userList[socket.id];
    }

    findUser(userName) {
        for(let key in this.userList) {
            if(this.userList[key].name == userName) {
                return this.userList[key];
            }
        }

        return undefined;
    }
}

module.exports = {
    Users: new Users()
}