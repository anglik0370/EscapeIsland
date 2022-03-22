const SocketState = require('../SocketState.js');
const Vector2 = require('../Vector2.js');
const {Users} = require('../Users.js');

function loginHandler(socket,payload) {
    if(payload.name === ""){
        sendError("이름을 입력해주세요", socket);
        return;
    }
    let userData = login(payload,socket);
    userData.position = Vector2.zero;
    userData.isImposter = false;
    userData.isDie = false;
    userData.voteNum = 0;
    userData.voteComplete = false;


    Users.userList[socket.id] = userData;
}


function login(data,socket,isTest = false) {
    if(!isTest) {
        //data = JSON.parse(data);
        const {name,socketId} = data;

        socket.state = SocketState.IN_LOBBY;

        //let position = respawnPoint[Math.floor(Math.random() * respawnPoint.length)];

        let sendData = {
            socketId:socket.id,
            name,
            roomNum:0
        };

        let payload = JSON.stringify(sendData);
        let type = "LOGIN";
        socket.send(JSON.stringify({type,payload}));

        return sendData;
    }
    else {

        let sendData = {
            socketId:socket.id,
            name:"test",
            roomNum:socket.room
        };

        return sendData;
    }
    

   
}

module.exports = {
    type:"LOGIN",
    act(socket,data) {
        loginHandler(socket,data);
    },
    test(socket,data) {
        login(data,socket,true);
    }
}