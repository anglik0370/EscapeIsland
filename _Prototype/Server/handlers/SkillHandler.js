const {Rooms} = require('../Rooms.js');
const CharacterType = require('../Utils/CharacterType.js');

module.exports = {
    type:"SKILL",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        if(data.skillType == CharacterType.Leon) {
            data.itemId = room.userList[data.targetId].getItem();
        }
        else if(data.skillType == CharacterType.Simon) {
            room.storageItemList[data.team].removeItemAmount(data.itemId);
        }
        else if(data.skillType == CharacterType.Joshua) {
            for(let i = 0; i < data.targetIdList.length; i++) {
                let itemId = room.userList[data.targetIdList[i]].getItem();
                data.itemIdList.push(itemId);
            }
        }

        room.broadcast(JSON.stringify({type:"SKILL",
        payload:JSON.stringify(data)}));
    }
}