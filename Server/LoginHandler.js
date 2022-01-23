const SocketState = require('./SocketState.js');
const Vector2 = require('./Vector2.js');

// let respawnPoint = [
//     new Vector3(3,5,0),
//     new Vector3(5,4,0),
//     new Vector3(7,2,0)
// ];

function LoginHandler(data,socket) {
    data = JSON.parse(data);
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

module.exports = LoginHandler;