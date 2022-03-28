const {Users} = require('./Users.js');
const WebSocket = require('ws');
const InGameTimer = require('./InGameTimer.js');
const InVoteTimer = require('./InVoteTimer.js');
const SetSpawnPoint = require('./GameSpawnHandler.js');
const SocketState = require('./SocketState.js');
const GetRandomPos = require('./SpawnPoint.js');
const sendError = require('./SendError.js');

class Rooms {
    constructor() {
        this.roomList = {};
        this.roomIdx = 1;
    }

    findRoom(roomName) {
        for(let key in this.roomList) {
            if(this.roomList[key].roomName == roomName) {
                return this.roomList[key];
            }
        }
        return undefined;
    }

    removeAllRoom() {
        this.roomIdx = 1;
    }

    getRoom(roomNum) {
        return this.roomList[roomNum];
    }

    createRoom(socket,roomInfo) {
        if(socket.state !== SocketState.IN_LOBBY){
            sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
            return;
        }
    
        if(roomInfo.name === ""){
            sendError("방이름을 입력해 주세요.", socket);
            return;
        }
        
        if(this.findRoom(roomInfo.name) !== undefined) {
            sendError("중복된 방 이름 입니다.",socket);
            return;
        }
        
    
        let r = new Room(roomInfo.name,this.roomIdx,1,roomInfo.userNum,roomInfo.kidnapperNum,false);
        socket.room = this.roomIdx;

        this.roomList[this.roomIdx] = r;

        if(Users.isTestServer) {
            this.roomList[this.roomIdx].inVoteTimer.setTimeToNextSlot(10);
        }

        this.join(socket,true);
    
        this.roomIdx++;
    }

    joinRoom(socket,roomNum) {
        if(socket.state !== SocketState.IN_LOBBY){
            sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
            return;
        }
        let room = this.roomList[roomNum];
    
        if(room === undefined || room.curUserNum >= room.userNum || room.playing) {
            sendError("들어갈 수 없는 방입니다.",socket);
            return;
        }

        socket.room = roomNum;

        this.join(socket,false);
        room.curUserNum++;
    }

    exitRoom(socket,roomNum) {
        if(socket.state !== SocketState.IN_ROOM && socket.state !== SocketState.IN_PLAYING){
            sendError("잘못된 접근입니다.", socket);
            return;
        }
        this.exit(socket,roomNum);
   
        socket.send(JSON.stringify({type:"EXIT_ROOM"}));
    
        socket.server.clients.forEach(soc=>{
            if(soc.room === roomNum) { //소켓이 해제된 유저가 있었던 방에 있다면
                this.roomBroadcast(this.roomList[roomNum]);
            }
            if(soc.state === SocketState.IN_LOBBY) {
                this.refreshRoom(soc);
            }
        });
    }
    
    exit(socket, roomNum) //방에서 나갔을 때의 처리
    {
        let room = this.roomList[roomNum]; //해당 방 받아오기
        if(room === undefined) return;
        
        let user = room.userList[socket.id];
    
        socket.room = 0; //나왔으니 룸 초기화
    
        socket.state = SocketState.IN_LOBBY; //방에서 나왔으니 state 바꿔주고
        room.curUserNum--; //그 방의 인원수--;
        room.removeSocket(socket.id);

        if(user !== undefined){ 
            // 초기화
            
            user.roomNum = 0;
            user.master = false; 
            user.isImposter = false;
            user.isDie = false;
            user.voteNum = 0;
            user.voteComplete = false;
        }
        
        
        if(room.curUserNum <= 0){ //사람이 0명일때 room delete
            room.initRoom();
            delete this.roomList[roomNum];
            return;
        }
    
        if(user.master && room.curUserNum > 0) { //마스터가 나갔을때 방장권한을 넘겨주기
            let keys = Object.keys(room.userList);
            room.userList[keys[0]].master = true;
        }
        
        this.roomBroadcast(roomNum);
        
        room.socketList.forEach(soc => {
            if(soc.id === socket.id) return;
            soc.send(JSON.stringify({type:"DISCONNECT",payload:socket.id}))
        });
    }

    join(socket,isMaster) {
        let user = Users.userList[socket.id];
        if(user === undefined) return;

        this.roomList[socket.room].addSocket(socket,user);

        socket.state = SocketState.IN_ROOM;

        if(user !== undefined){
            //user.roomNum = this.roomIdx;
            user.roomNum = socket.room;
            user.master = isMaster;
            user.position = GetRandomPos();
        }

        socket.send(JSON.stringify({type:"ENTER_ROOM"}));

        if(isMaster)
            setTimeout(() => this.roomBroadcast(socket.room),200);

        socket.server.clients.forEach(soc=>{
            if(soc.state != SocketState.IN_LOBBY) 
                return;
            this.refreshRoom(soc);
        });
    }

    refreshRoom(socket) //룸정보 갱신
    {
        let value = Object.values(this.roomList);
        let dataList = [];
        for(let i = 0; i < value.length; i++) {
            dataList.push(value[i].returnData());
        }
        socket.send(JSON.stringify({type:"REFRESH_ROOM", payload:JSON.stringify({dataList})})); 
    }

    roomBroadcast(roomNum,sendType = "REFRESH_MASTER") {
        let room = this.roomList[roomNum];

        if(room === undefined) return;
    
        let dataList = Object.values(room.userList); // 전송할 배열

        room.broadcast(JSON.stringify({type:sendType,payload:JSON.stringify({dataList})}));
    }

    allRoomBroadcast() {
        let keys = Object.keys(this.roomList);
        for(let i = 0; i < keys.length; i++) {
            let room = this.roomList[keys[i]];
            let dataList = Object.values(room.userList);

            room.socketList.forEach(soc => {
                soc.send(JSON.stringify({type:"REFRESH_USER",payload:JSON.stringify({dataList})}));
            });
        }
    }

    startServer() {
        setInterval(() => {
            this.allRoomBroadcast(this.roomList);
        },200);
    }
    
}

class Room {
    constructor(roomName,roomNum,curUserNum,userNum,kidnapperNum,playing) {
        this.roomName = roomName;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.kidnapperNum = kidnapperNum;
        this.playing = playing;

        this.inGameTimer = new InGameTimer();
        this.inVoteTimer = new InVoteTimer();
        this.curTimer = undefined;

        this.skipCount = 0;
        
        this.interval = 1000;
        this.nextTime = 0;
        this.expected = Date.now();

        this.socketList = [];
        this.userList = {};

        this.isEnd = true;
    }

    voteEnd() {
        let keys = Object.keys(this.userList);
        let allComplete = true;
        let targetSocIdArr = [];
    
        for(let i = 0; i < keys.length; i++) {
            //안죽었을때 & 투표완료했을때 넘어가야함
            if((!this.userList[keys[i]].isDie && !this.userList[keys[i]].voteComplete)) {
                allComplete = false;
                break;
            }
        }
        
        if(allComplete) {
            let dummy = -1;
    
            for(let i = 0; i < keys.length; i++) {
                let user = this.userList[keys[i]];
                let idx = user.voteNum;
    
                if(dummy != 0 &&  idx == dummy) {
                    targetSocIdArr.push(user.socketId);
                }
                else if(idx > dummy && idx > this.skipCount) {
                    dummy = idx;
                    targetSocIdArr.length = 0;
                    targetSocIdArr.push(user.socketId);
                }
    
                this.userList[keys[i]].voteNum = 0;
                this.userList[keys[i]].voteComplete = false;
            }
            this.skipCount = 0;
            this.inVoteTimer.initTime();
            
            if(targetSocIdArr.length == 1) {
                this.broadcast(JSON.stringify({type:"VOTE_DIE",payload:targetSocIdArr[0]}));
                this.userList[targetSocIdArr[0]].isDie = true;
    
                //납치자를 모두 찾았을때
    
                let keys = Object.keys(this.userList);
                let posList = SetSpawnPoint(keys.length);
    
                for(let i = 0; i < keys.length; i++) {
                    this.userList[keys[i]].position = posList[i];
                }
                
                let dataList = Object.values(this.userList);
                let filteredArr = dataList.filter(user => user.isImposter && !user.isDie);
    
                if(filteredArr.length <= 0) {
                    this.broadcast(JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:1})}),true);
                    this.initRoom();
                    return true;
                }

                if(this.kidnapperWinCheck()) {
                    this.initRoom();
                    return true;
                }
    
            }
            this.voteTimeEnd();
            return true;
        }

        return false;
    }

    voteTimeEnd() {
        if(this.isEnd) {
            this.changeTime();
            console.log("changeTime - voteTimeEnd");
        }
        else{
            this.broadcast(JSON.stringify({type:"VOTE_TIME_END",payload:""}));

            this.startTimer();
        }
    }

    kidnapperWinCheck() {
        let imposterCount = 0;
        let citizenCount = 0;
    
        let keys = Object.keys(this.userList);
    
        for(let i = 0; i < keys.length; i++) {
            if(this.userList[keys[i]].isDie) continue;
    
            if(this.userList[keys[i]].isImposter) imposterCount++;
            else citizenCount++;
        }
    
        let posList = SetSpawnPoint(keys.length);
    
        for(let i = 0; i < keys.length; i++) {
            this.userList[keys[i]].position = posList[i];
        }
        let dataList = Object.values(this.userList);
    
        //살아있는 임포가 시민보다 많을 경우
        if(imposterCount >= citizenCount) {
            //임포승
            console.log("다주것다");
            this.broadcast(JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList,gameOverCase:0})}),true);
            this.initRoom();
            return true;
        }
        console.log("아직 남았다");
        return false;
        //테스트용 코드 
        // let dataList = Object.values(room.userList);
    
        // broadcast(socket,JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList})}));
        // room.initRoom();
    }

    gameStart(socket) {
        if(socket.state !== SocketState.IN_ROOM){
            sendError("방이 아닌 곳에서 시도를 하였습니다.", socket);
            return;
        }
    
        if(this.curUserNum < 2) {
            sendError("최소 2명 이상의 인원이 있어야 합니다.",socket);
            return;
        }
    
        if(this.curUserNum <= this.kidnapperNum) {
            sendError("현재 유저의 수가 납치자의 수보다 같거나 적습니다.",socket);
            return;
        }
    
        let keys = Object.keys(this.userList);
        let imposterLength = this.kidnapperNum;
        let idx;
    
        for(let i = 0; i < imposterLength; i++) {
            do {
                idx = Math.floor(Math.random() * keys.length);
            }while(this.userList[keys[idx]].isImposter)
    
            this.userList[keys[idx]].isImposter = true;
        }
    
        //테스트용 코드
        // if(this.userList[socket.id] !== undefined) {
        //     this.userList[socket.id].isImposter = true;
        // }
    
        //Rooms.roomBroadcast(this.roomNum);
        //let d = Object.values(this.userList);
        //this.broadcast(JSON.stringify({type:"REFRESH_MASTER",payload:JSON.stringify({dataList:d})}));
    
        //룸에 있는 플레이어들의 포지션 조정
    
        let posList = SetSpawnPoint(keys.length);
    
        for(let i = 0; i < keys.length; i++) {
            this.userList[keys[i]].position = posList[i];
        }
        
        let dataList = Object.values(this.userList);
    
        this.playing = true;
        this.startTimer();
        this.broadcast(JSON.stringify({type:"GAME_START",payload:JSON.stringify({dataList})}));
    }
    
    initRoom() {
        console.log("initRoom");
        this.playing = false;
        this.stopTimer();
        this.skipCount = 0;
        this.inGameTimer = new InGameTimer();
        this.inVoteTimer = new InVoteTimer();

        if(Users.isTestServer) {
            this.roomList[this.roomIdx].inVoteTimer.setTimeToNextSlot(10);
        }

        for(let key in this.userList) {
            this.userList[key].isDie = false;
            this.userList[key].isImposter = false;
            this.userList[key].voteNum = 0;
            this.userList[key].voteComplete = false;
        }

    }

    startTimer() {
        //this.skipCount = 0;
        this.stopTimer();
        this.expected = Date.now() + 1000; //현재시간 + 1초
        this.curTimer = setTimeout(this.rTimer.bind(this),this.interval);
    }

    startVoteTimer() {
        //this.inVoteTimer.initTime();
        this.stopTimer();
        this.curTimer = setTimeout(this.voteTimer.bind(this),this.interval,false);
    }

    stopTimer() {
        clearTimeout(this.curTimer);
    }

    rTimer() {
        let dt = Date.now() - this.expected; //현재 시간 - 시작시간

        if(this.inGameTimer.timeRefresh(this.socketList)) {

            let keys = Object.keys(this.userList);
            let posList = SetSpawnPoint(keys.length);

            for(let i = 0; i< keys.length; i++) {
                this.userList[keys[i]].position = posList[i];
            }

            let dataList = Object.values(this.userList);

            if(this.inGameTimer.isEndGame) {
                this.broadcast(JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:2})}),true);
                this.initRoom();
                return;
            }
            
            this.broadcast(JSON.stringify({type:"VOTE_TIME",payload:JSON.stringify({dataList,type:2})}));

            this.expected = Date.now() + 1000;
            this.curTimer = setTimeout(this.voteTimer.bind(this),this.interval,true);
            return;
        }

        this.expected += this.interval;

        this.nextTime = Math.max(0,this.interval - dt);
        this.curTimer = setTimeout(this.rTimer.bind(this),this.nextTime);
    }

    changeTime() {
        this.inVoteTimer.initTime();
        //this.skipCount = 0;
        let p = this.inGameTimer.returnPayload();
        console.log("time refresh - changeTime");
        this.broadcast(JSON.stringify({type:"TIME_REFRESH",payload:p}));
        this.startTimer();
    }

    voteTimer(isEnd) {
        let dt = Date.now() - this.expected;
        this.isEnd = isEnd;
        if(this.inVoteTimer.timeRefresh(this.socketList)) {
            if(!this.voteEnd()) {
                this.voteTimeEnd();
                console.log("changeTime - voteTimer");
            }
            return;
        }

        this.expected += this.interval;

        this.nextTime = Math.max(0,this.interval - dt);
        this.curTimer = setTimeout(this.voteTimer.bind(this),this.nextTime,isEnd);
    }

    addSocket(socket,user) {
        this.socketList.push(socket);
        this.userList[user.socketId] = user;
    }

    removeSocket(rSocketIdx) {
        let idx = this.socketList.findIndex(soc => soc.id == rSocketIdx);
        this.socketList.splice(idx,1);
        delete this.userList[rSocketIdx];
    }

    returnData() {
        let data = {name:this.roomName,roomNum:this.roomNum,curUserNum:this.curUserNum,userNum:this.userNum,kidnapperNum:this.kidnapperNum,playing:this.playing};
        return data;
    }

    broadcast(msg,isEnd = false) {
        this.socketList.forEach(soc => {
            if(soc.readyState !== WebSocket.OPEN) return;

            if(isEnd) soc.state = SocketState.IN_ROOM;
            soc.send(msg);
        });
    }

}

module.exports = {
    Rooms: new Rooms()//,Room
}