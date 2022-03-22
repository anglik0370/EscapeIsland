const {Rooms} = require('../Rooms.js');

module.exports = {
    type:"STORAGE_FULL",
    act(socket,data) {
        let room = Rooms.getRoom(socket.room);

        if(room === undefined) return;

        if(room.inGameTimer.isLightTime) {
            room.inGameTimer.isEndGame = true;
            room.broadcast(JSON.stringify({type:"STORAGE_FULL",payload:"저녁까지 쳐 버티도록 하세요"}));
        }
        else {
            let keys = Object.keys(room.userList);
            let posList = SetSpawnPoint(keys.length);
    
            for(let i = 0; i < keys.length; i++) {
                userList[keys[i]].position = posList[i];
            }
    
            let dataList = Object.values(room.userList);
    
            // 납치자가 있었을때 배에 탔다면의 경우인데 폐기 됨. (혹시 모르니 주석처리만)
            // let filteredList = dataList.filter(user => user.isImposter && !user.isDie); 
            // let type = filteredList.length > 0 ? "WIN_KIDNAPPER" : "WIN_CITIZEN";
    
            //다 모아서 탈출 시 시민 승
            room.broadcast(JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:2})}),true);
            room.initRoom();
        }
    }
}