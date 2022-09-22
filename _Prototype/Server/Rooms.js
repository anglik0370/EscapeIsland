const WebSocket = require('ws');
const {Users} = require('./Users.js');
const Room = require('./Room.js');

const regex = require('./Utils/Regex.js');
const SocketState = require('./Utils/SocketState.js');
const team = require('./Utils/Team.js');
const GetRandomPos = require('./Utils/SpawnPoint.js');
const sendError = require('./Utils/SendError.js');

class Rooms {
    constructor() {
        this.roomList = {};
        this.roomIdx = 1;
    }

    findRoom(roomName,isCreate = false) {
        if(isNaN(roomName)) {
            for(let key in this.roomList) {
                if(this.roomList[key].roomName === roomName) {
                    return this.roomList[key];
                }
            }
        }
        else {
            if(!isCreate) {
                for(let key in this.roomList) {
                    if(this.roomList[key].roomNum === parseInt(roomName)) {
                        return this.roomList[key];
                    }
                }
            }
        }
        return undefined;
    }

    removeAllRoom(socket) {
        this.roomIdx = 1;
        this.roomList = {};

        this.refreshRoom(socket);
    }

    getRoom(roomNum) {
        return this.roomList[roomNum];
    }

    createRoom(socket,roomInfo) {
        if(socket.state !== SocketState.IN_LOBBY){
            sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
            return;
        }
    
        if(!roomInfo.name.match(regex.roomRegex())){
            sendError("방이름은 한글, 영어, 숫자 15자내로만 구성될 수 있습니다.", socket);
            return;
        }
        
        if(this.findRoom(roomInfo.name,true) !== undefined) {
            sendError("중복된 방 이름 입니다.",socket);
            return;
        }
        
    
        let r = new Room(roomInfo.name,this.roomIdx,0,roomInfo.userNum,false);
        socket.room = this.roomIdx;

        this.roomList[this.roomIdx] = r;

        this.join(socket,true);

        this.roomIdx++;
    }

    joinRoom(socket,roomNum) {
        if(socket.state !== SocketState.IN_LOBBY){
            sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
            return;
        }
        let room = this.roomList[roomNum];
    
        if(room === undefined || room.curUserNum >= room.userNum || room.playing || room.isTestRoom()) {
            sendError("들어갈 수 없는 방입니다.",socket);
            return;
        }

        socket.room = roomNum;

        this.join(socket,false);
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
        if(user === undefined) return;

        if(room.areaList[user.area] !== undefined) {
            room.areaList[user.area].removeUserList(user.curTeam == team.BLUE,user.charId == 7);
        }

        let master = user.master;
    
        socket.room = 0; //나왔으니 룸 초기화
    
        socket.state = SocketState.IN_LOBBY; //방에서 나왔으니 state 바꿔주고
        room.curUserNum--; //그 방의 인원수--;
        room.removeSocket(socket.id);

        if(user !== undefined){ 
            // 초기화
            user.initExitData();
        }

        if(room.isTestRoom()) {
            for(let i = 0; i < room.socketList.length; i++) {
                if(room.socketList[i] !== undefined)
                {
                    room.socketList[i].close();
                }
            }
            room.initRoom();
            delete this.roomList[roomNum];
            return;
        }
        
        if(room.curUserNum <= 0){ //사람이 0명일때 room delete
            room.initRoom();
            delete this.roomList[roomNum];
            return;
        }
    
        if(master && room.curUserNum > 0) { //마스터가 나갔을때 방장권한을 넘겨주기
            let keys = Object.keys(room.userList);
            room.userList[keys[0]].master = true;
        }
        
        room.refreshUserCount();
        this.roomBroadcast(roomNum);
        
        room.socketList.forEach(soc => {
            if(soc.id === socket.id) return;
            soc.send(JSON.stringify({type:"DISCONNECT",payload:socket.id}))
        });

        //게임 종료처리
        if(room.playing)
        {
            let blueTeamLength = 0;
            let redTeamLength = 0;
    
            for(let key in room.userList) {
                if(room.userList[key].curTeam == team.NONE) continue;
    
                if(room.userList[key].curTeam == team.BLUE) {
                    blueTeamLength++;
                }
                else {
                    redTeamLength++;
                }
            }

            if(redTeamLength == 0 || blueTeamLength == 0) {
                room.gameEnd(redTeamLength == 0 ? team.BLUE : team.RED);
            }
        }
    }

    join(socket,isMaster) {
        let user = Users.userList[socket.id];
        if(user === undefined) return;

        let room = this.roomList[socket.room];

        room.addSocket(socket,user);
        room.setTimersTime(socket);

        socket.state = SocketState.IN_ROOM;

        if(user !== undefined){
            user.roomNum = socket.room;
            user.master = isMaster;

            let userList = Object.values(room.userList);

            let blueTeamList = userList.filter(user => user.curTeam == team.BLUE);
            user.curTeam = blueTeamList.length < userList.length / 2 ? team.BLUE : team.RED;
            user.position = GetRandomPos();
        }
        
        if(!user.testClient) 
        socket.send(JSON.stringify({type:"ENTER_ROOM",payload:""}));

        setTimeout(() => {
            this.roomBroadcast(socket.room);
            room.refreshUserCount();
        },100);
        
        if(!user.testClient) 
        socket.server.clients.forEach(soc=>{
            if(soc.state === SocketState.IN_LOBBY) 
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
        for(let key in this.roomList) {
            let room = this.roomList[key];
            let dataList = Object.values(room.userList);

            room.broadcast(JSON.stringify({type:"REFRESH_USER",payload:JSON.stringify({dataList})}));
            room.initVoiceData();
        }
    }

    roomAreaListBroadCast() {
        for(let key in this.roomList) {
            let room = this.roomList[key];

            if(room === undefined || !room.playing) continue;

            let areaDataList = room.getAreaListData();

            room.broadcast(JSON.stringify({type:"REFRESH_AREA",payload:JSON.stringify({isOpen:true,areaDataList})}));
        }
    }

    startServer() {
        setInterval(() => {
            this.allRoomBroadcast();
        },100);

        setInterval(() => {
            this.roomAreaListBroadCast();
        },200);
    }
    
}

module.exports = {
    Rooms: new Rooms()
}