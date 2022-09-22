const {Rooms} = require('../Rooms.js');
const {Users} = require('../Users.js');
const WebSocket = require('ws');
const SocketState = require('../Utils/SocketState.js');
const _ = require('lodash');
const GetRandomPos = require('../Utils/SpawnPoint.js');
const team = require('../Utils/Team.js');

let testIdx = 10000;

module.exports =  {
    type:"TEST_CLIENT",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;
        
        for(let i = 0; i < 3; i++) {
            let dummySocket = new WebSocket("ws://localhost:31012");
            let testUserData = _.cloneDeep(Users.userList[socket.id]);
            dummySocket.id = 0;
            dummySocket.id = ++testIdx;
    
            dummySocket.state = SocketState.IN_ROOM;
            dummySocket.room = socket.room;
    
            Users.connectedSocket[dummySocket.id] = dummySocket;
    
            let user = Users.userList[dummySocket.id] = testUserData;
    
            user.master = false;
            user.name = `test${dummySocket.id - 10000}`;
            user.socketId = dummySocket.id;
            user.position = GetRandomPos();
            user.curTeam = i % 2 == 0 ? team.RED : team.BLUE;
            user.area = 16;
            user.ready = true;
            user.testClient = true;
            
            Rooms.join(dummySocket,false);
            //room.addSocket(dummySocket,Users.userList[dummySocket.id]);
    
        }
       
        socket.server.clients.forEach(soc => {
            if(soc.state === SocketState.IN_LOBBY) {
                Rooms.refreshRoom(soc);
            }
        });
    }
}