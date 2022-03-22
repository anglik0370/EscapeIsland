const WebSocket = require('ws');
const port = 31012;

const {Rooms} = require('./Rooms.js');
const {Users} = require('./Users.js');
const SocketState = require('./SocketState.js');
const fs = require('fs');

let handlers = {};

fs.readdir("./handlers",(err,files) => {
    files.forEach(file => {
        let handler = require(`./handlers/${file}`);
        handlers[handler.type] = handler;
    });
});

const wsService = new WebSocket.Server({port}, ()=>{
    console.log(`웹 소켓이 ${port}에서 구동중`);
    
});


wsService.on("connection", socket => {
    console.log("소켓 연결");
    Users.connect(socket,wsService);

    socket.on("close", () => {
        console.log(`소켓 연결 해제 id: ${socket.id}`);
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
        delete Users.connectedSocket[socket.id];
        delete Users.userList[socket.id];
    });

    socket.on("message",  msg => {
        //여기서는 클라이언트에서 받은 메시지를 처리해줘야 한다.
        try {
            onMessage(socket,msg);
        }
        catch (error) {
            console.log(`잘못된 요청 발생 : ${msg}`);
            console.log(error);
        }
    });

});

function onMessage(socket,msg) {
    const data = JSON.parse(msg);

    //if(socket.readyState ==)

    if(handlers[data.type] !== undefined) {
        if(isJsonString(data.payload)) {
            handlers[data.type].act(socket,JSON.parse(data.payload));
        }
        else {
            handlers[data.type].act(socket,data.payload);
        }
    }
}

function isJsonString(str) {
    try {
        let js = JSON.parse(str);
        return (typeof js === 'object');
    } catch(e) {
        return false;
    }
}

Rooms.startServer();