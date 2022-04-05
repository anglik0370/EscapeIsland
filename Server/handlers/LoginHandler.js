const SocketState = require('../Utils/SocketState.js');
const Player = require('../Player.js');
const {Users} = require('../Users.js');
const getRegex = require('../Utils/Regex.js');
const sendError = require('../Utils/SendError.js');

function loginHandler(socket,payload) {
    if(!payload.name.match(getRegex())){
        sendError("이름은 한글, 영어, 숫자 15자내로만 구성될 수 있습니다.", socket);
        return;
    }

    if(Users.findUser(payload.name) !== undefined) {
        sendError("중복된 이름이 있습니다.",socket);
        return;
    }
    
    let userData = login(payload,socket);

    Users.userList[socket.id] = userData;
}


function login(data,socket,isTest = false) {
    if(!isTest) {
        //data = JSON.parse(data);
        const {name,socketId} = data;

        socket.state = SocketState.IN_LOBBY;

        //let position = respawnPoint[Math.floor(Math.random() * respawnPoint.length)];
        let user = new Player();
        user.initData(socket.id,name,0);

        let sendData = {
            socketId:socket.id,
            name,
            roomNum:0
        };

        let payload = JSON.stringify(sendData);
        let type = "LOGIN";
        socket.send(JSON.stringify({type,payload}));

        return user;
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