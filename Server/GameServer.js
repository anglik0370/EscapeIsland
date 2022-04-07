const WebSocket = require('ws');
const port = 31012;

const {Rooms} = require('./Rooms.js');
const {Users} = require('./Users.js');
const SocketState = require('./Utils/SocketState.js');
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
        Users.disconnect(socket,wsService,Rooms);
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

    if(socket.readyState !== WebSocket.OPEN) return;

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