const Users = require('../Users.js');

module.exports = {
    type:"TRANSFORM",
    act(socket,data) {
        if(Users.userList[data.socketId] !== undefined) {
            Users.userList[data.socketId].position = data.position;
        }
    }
}