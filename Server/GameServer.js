const WebSocket = require('ws');
const port = 31012;

const SocketState = require('./SocketState.js');
const Vector2 = require('./Vector2.js');
const Room = require('./Room.js');
const InGameTimer = require('./InGameTimer.js');
const LoginHandler = require('./LoginHandler.js');
const GetRandomPos = require('./SpawnPoint.js');
const SetSpawnPoint = require('./GameSpawnHandler.js');
const _ = require('lodash');

let socketIdx = 0;
let roomIdx = 1; 

let testIdx = 1000;

let roomList = {}; //룸들의 정보들을 담고있는 배열
let userList = {}; //유저들의 정보들을 담고있는 배열
let connectedSocket = {}; //연결되어있는 소켓들을 담고있는 배열

const wsService = new WebSocket.Server({port}, ()=>{
    console.log(`웹 소켓이 ${port}에서 구동중`);
});

wsService.on("connection", socket => {
    console.log("소켓 연결");

    socket.state = SocketState.IN_LOGIN;
    socket.id = socketIdx;
    connectedSocket[socketIdx] = socket;
    socketIdx++;
    socket.room = -1;

    socket.on("close", () => {
        console.log(`소켓 연결 해제 id: ${socket.id}`);
        let roomNum = socket.room; //현재 소켓의 룸 idx를 받아옴

        //현재 socket이 룸에 들어가 있다면
        if(socket.room > 0 && socket.state === SocketState.IN_ROOM) {
            exitRoom(socket,roomNum); //방 나가기
            wsService.clients.forEach(soc => {
                if(soc.room === roomNum) { //소켓이 해제된 유저가 있었던 방에 있다면
                    //refreshUser(soc,roomNum); 
                    roomBroadcast(roomList[roomNum]);
                }
                if(soc.state === SocketState.IN_LOBBY) {
                    refreshRoom(soc);
                }
            });
        }
        //현재 socket의 state가 IN_PLAYING일때
        if(socket.state === SocketState.IN_PLAYING) {
            if(socket.room > 0) {
                exitRoom(socket,roomNum);
            }
            wsService.clients.forEach(soc=>{
                if(soc.state === SocketState.IN_LOBBY) {
                    refreshRoom(soc);
                }
                if(soc.room === roomNum) {
                    roomBroadcast(roomList[roomNum]);
                }
            })

        }
        delete connectedSocket[socket.id];
        delete userList[socket.id];
    });

    socket.on("message",  msg => {
        //여기서는 클라이언트에서 받은 메시지를 처리해줘야 한다.
        try {
            const data = JSON.parse(msg);
            switch (data.type) {
                case "LOGIN":
                    login(socket,JSON.parse(data.payload));
                    break;
                case "TRANSFORM":
                    let transformVO = JSON.parse(data.payload);

                    if(userList[transformVO.socketId] !== undefined) {
                        userList[transformVO.socketId].position = transformVO.position;
                    }
                    break;
                case "ROOM_REFRESH_REQ":
                    refreshRoom(socket);
                    break;
                case "CREATE_ROOM":
                    roomCreate(socket,JSON.parse(data.payload));
                    break;
                case "JOIN_ROOM":
                    roomJoin(socket,JSON.parse(data.payload).roomNum);
                    break;
                case "TEST_CLIENT":
                    //userList[socket.id].isImposter = true;
                    testClient(socket);
                    break;
                case "EXIT_ROOM":
                    roomExit(socket,JSON.parse(data.payload).roomNum);
                    break;
                case "GameStart":
                    gameStart(socket,JSON.parse(data.payload));
                    break;
                case "KILL":
                    kill(socket,JSON.parse(data.payload));
                    break;
                case "GET_ITEM":
                    let spawnerId = JSON.parse(data.payload).spawnerId;

                    broadcast(socket,JSON.stringify({type:"GET_ITEM",payload:spawnerId}));
                    break;
                case "STORAGE_DROP":
                    let itemSOId = JSON.parse(data.payload).itemSOId;

                    broadcast(socket,JSON.stringify({type:"STORAGE_DROP",payload:itemSOId}));
                    break;
                case "STORAGE_FULL":
                    storageFull(socket);
                    break;
                case "START_REFINERY":
                    let startData = JSON.parse(data.payload);

                    broadcast(socket,JSON.stringify({type:"START_REFINERY",payload:JSON.stringify({refineryId:startData.refineryId,itemSOId:startData.itemSOId})}))

                    break;
                case "RESET_REFINERY":
                    let resetRefineryId = JSON.parse(data.payload).refineryId;

                    broadcast(socket,JSON.stringify({type:"RESET_REFINERY",payload:resetRefineryId}));
                    break;
                case "TAKE_REFINERY":
                    let takeRefineryId = JSON.parse(data.payload).refineryId;

                    broadcast(socket,JSON.stringify({type:"TAKE_REFINERY",payload:takeRefineryId}));
                    break;
                case "CHAT":
                    broadcast(socket,JSON.stringify({type:"CHAT",payload:data.payload}))
                    break;
                case "VOTE_COMPLETE":
                    voteComplete(socket,JSON.parse(data.payload));
                    break;
                case "EMERGENCY":
                    deadReportOrEmergency(socket,0);
                    break;
                case "DEAD_REPORT":
                    deadReportOrEmergency(socket,1);
                    break;
            }
        }
        catch (error) {
            console.log(`잘못된 요청 발생 : ${msg}`);
            console.log(error);
        }
    });

});

function login(socket,payload) {
    if(payload.name === ""){
        sendError("이름을 입력해주세요", socket);
        return;
    }
    let userData = LoginHandler(payload,socket);
    userData.position = Vector2.zero;
    userData.isImposter = false;
    userData.isDie = false;
    userData.voteNum = 0;
    userData.voteComplete = false;


    userList[socket.id] = userData;
}

function roomCreate(socket,roomInfo) {
    if(socket.state !== SocketState.IN_LOBBY){
        sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
        return;
    }

    if(roomInfo.name === ""){
        sendError("방이름을 입력해 주세요.", socket);
        return;
    }

    //roomList[roomIdx] = {name:roomInfo.name, roomNum:roomIdx,curUserNum:1,userNum:roomInfo.userNum,playing:false};
    let r = new Room(roomInfo.name,roomIdx,1,roomInfo.userNum,roomInfo.kidnapperNum,false);
    r.inGameTimer = new InGameTimer();
    socket.state = SocketState.IN_ROOM;
    socket.room = roomIdx;

    if(userList[socket.id] !== undefined){
        userList[socket.id].roomNum = roomIdx;
        userList[socket.id].master = true;
        userList[socket.id].position = GetRandomPos();
    }

    r.addSocket(socket, userList[socket.id]);
    roomList[roomIdx] = r;

    roomBroadcast(roomList[roomIdx]);

    socket.send(JSON.stringify({type:"ENTER_ROOM"}));

    wsService.clients.forEach(soc=>{
        if(soc.state != SocketState.IN_LOBBY) 
            return;
        refreshRoom(soc);
    });
    roomIdx++;
}

function testClient(socket) {
    // if(userList[socket.id] !== undefined)
    //     userList[socket.id].isImposter = true;

    let room = roomList[socket.room];

    if(room === undefined) return;
    
    for(let i = 0; i < 3; i++) {
        let dummySocket = new WebSocket("ws://localhost:31012");
        let testUserData = _.cloneDeep(userList[socket.id]);
        dummySocket.id = 0;
        dummySocket.id = ++testIdx + dummySocket.id;

        dummySocket.state = SocketState.IN_ROOM;
        dummySocket.room = socket.room;

        connectedSocket[dummySocket.id] = dummySocket;

        userList[dummySocket.id] = testUserData;

        userList[dummySocket.id].master = false;
        userList[dummySocket.id].name = `test${dummySocket.id - 1000}`;
        userList[dummySocket.id].socketId = dummySocket.id;
        userList[dummySocket.id].position = GetRandomPos();
        
        room.curUserNum++;
        room.addSocket(dummySocket,userList[dummySocket.id]);

    }
   
    wsService.clients.forEach(soc => {
        if(soc.state === SocketState.IN_LOBBY) {
            refreshRoom(soc);
        }
    });
}

function roomJoin(socket,roomNum) {
    if(socket.state !== SocketState.IN_LOBBY){
        sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
        return;
    }
    let room = roomList[roomNum];

    if(room === undefined || room.curUserNum >= room.userNum || room.playing) {
        sendError("들어갈 수 없는 방입니다.");
    }
    socket.room = roomNum;

    if(userList[socket.id] !== undefined) {
        userList[socket.id].roomNum = roomNum;
        userList[socket.id].master = false;
        userList[socket.id].position = GetRandomPos();
    }

    socket.state = SocketState.IN_ROOM;
    room.curUserNum++;
    room.addSocket(socket,userList[socket.id]);

    socket.send(JSON.stringify({type:"ENTER_ROOM"}));

    wsService.clients.forEach(soc => {
        if(soc.state === SocketState.IN_LOBBY) {
            refreshRoom(soc);
        }
    });
}

function roomExit(socket,roomNum) {
    if(socket.state !== SocketState.IN_ROOM && socket.state !== SocketState.IN_PLAYING){
        sendError("잘못된 접근입니다.", socket);
        return;
    }

    exitRoom(socket,roomNum);

    socket.send(JSON.stringify({type:"EXIT_ROOM"}));

    wsService.clients.forEach(soc=>{
        if(soc.room === roomNum) { //소켓이 해제된 유저가 있었던 방에 있다면
            roomBroadcast(roomList[roomNum]);
        }
        if(soc.state === SocketState.IN_LOBBY) {
            refreshRoom(soc);
        }
    });
}

function gameStart(socket,payload) {
    if(socket.state !== SocketState.IN_ROOM){
        sendError("방이 아닌 곳에서 시도를 하였습니다.", socket);
        return;
    }
    let room = roomList[payload.roomNum];

    if(room.curUserNum < 2) {
        sendError("최소 2명 이상의 인원이 있어야 합니다.",socket);
        return;
    }

    if(room.curUserNum <= room.kidnapperNum) {
        sendError("현재 유저의 수가 납치자의 수보다 같거나 적습니다.",socket);
        return;
    }

    let keys = Object.keys(room.userList);
    let imposterLength = room.kidnapperNum;
    let idx;

    for(let i = 0; i < imposterLength; i++) {
        do {
            idx = Math.floor(Math.random() * keys.length);
        }while(userList[keys[idx]].isImposter)

        userList[keys[idx]].isImposter = true;
    }

    //테스트용 코드
    // if(userList[socket.id] !== undefined) {
    //     userList[socket.id].isImposter = true;
    // }

    roomBroadcast(room);

    //룸에 있는 플레이어들의 포지션 조정

    let posList = SetSpawnPoint(keys.length);

    for(let i = 0; i < keys.length; i++) {
        userList[keys[i]].position = posList[i];
    }
    
    let dataList = Object.values(room.userList);

    room.playing = true;
    room.startTimer();
    room.socketList.forEach(soc => {
        soc.state = SocketState.IN_PLAYING;
        //connectedSocket[soc.id].state = SocketState.IN_PLAYING;
        soc.send(JSON.stringify({type:"GAME_START",payload:JSON.stringify({dataList})}));
    });
}

function kill(socket,payload) {
    let room = roomList[socket.room];

    if(room === undefined) return;

    let socId = payload.targetSocketId;

    if(userList[socId] !== undefined) {
        userList[socId].isDie = true;
    }

    roomBroadcast(room,"KILL");

    //여기서 추가로 게임 승패 여부도 검사해야 할 듯?

    let imposterCount = 0;
    let citizenCount = 0;

    let keys = Object.keys(room.userList);

    for(let i = 0; i < keys.length; i++) {
        if(userList[keys[i]].isDie) continue;

        if(userList[keys[i]].isImposter) imposterCount++;
        else citizenCount++;
    }

    let posList = SetSpawnPoint(keys.length);

    for(let i = 0; i < keys.length; i++) {
        userList[keys[i]].position = posList[i];
    }
    let dataList = Object.values(room.userList);

    //살아있는 임포가 시민보다 많을 경우
    if(imposterCount >= citizenCount) {
        //임포승
        broadcast(socket,JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList,gameOverCase:0})}),true);
        room.initRoom();
    }

    //테스트용 코드
    // let dataList = Object.values(room.userList);

    // broadcast(socket,JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList})}));
    // room.initRoom();
}

function storageFull(socket) {
    let room = roomList[socket.room];

    if(room.inGameTimer.isLightTime) {
        room.inGameTimer.isEndGame = true;
        broadcast(socket,JSON.stringify({type:"STORAGE_FULL",payload:"저녁까지 쳐 버티도록 하세요"}));
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
        broadcast(socket,JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:2})}),true);
        room.initRoom();
    }
}

function voteComplete(socket,payload) {
    let room = roomList[socket.room];

    if(room === undefined) return;

    if(payload.voteTargetId === -1){
        room.skipCount++;
    }
    else {
        userList[payload.voteTargetId].voteNum++;
    }

    userList[payload.voterId].voteComplete = true;

    broadcast(socket,JSON.stringify({type:"VOTE_COMPLETE",payload:JSON.stringify({voterId:payload.voterId,voteTargetId:payload.voteTargetId})}));

    let keys = Object.keys(room.userList);
    let allComplete = true;
    let targetSocIdArr = [];

    for(let i = 0; i < keys.length; i++) {
        //안죽었을때 & 투표완료했을때 넘어가야함
        if((!userList[keys[i]].voteComplete && !userList[keys[i]].isDie)) {
            allComplete = false;
            break;
        }
    }
    
    if(allComplete) {
        let dummy = -1;

        for(let i = 0; i < keys.length; i++) {
            let idx = userList[keys[i]].voteNum;

            if(dummy != 0 &&  idx == dummy) {
                targetSocIdArr.push(userList[keys[i]].socketId);
            }
            else if(idx > dummy && idx > room.skipCount) {
                dummy = idx;
                targetSocIdArr.length = 0;
                targetSocIdArr.push(userList[keys[i]].socketId);
            }

            userList[keys[i]].voteNum = 0;
            userList[keys[i]].voteComplete = false;
        }
        room.skipCount = 0;
        room.inVoteTimer.initTime();
        
        if(targetSocIdArr.length == 1) {
            room.socketList.forEach(soc => {
                soc.send(JSON.stringify({type:"VOTE_DIE",payload:targetSocIdArr[0]}));
            });
            userList[targetSocIdArr[0]].isDie = true;

            //납치자를 모두 찾았을때

            let keys = Object.keys(room.userList);
            let posList = SetSpawnPoint(keys.length);

            for(let i = 0; i < keys.length; i++) {
                userList[keys[i]].position = posList[i];
            }
            
            let dataList = Object.values(room.userList);
            let filteredArr = dataList.filter(user => user.isImposter && !user.isDie);

            if(filteredArr.length <= 0) {
                broadcast(socket,JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList,gameOverCase:1})}),true);
                room.initRoom();
                return;
            }

            let imposterCount = 0;
            let citizenCount = 0;
        
            for(let i = 0; i < keys.length; i++) {
                if(userList[keys[i]].isDie) continue;
        
                if(userList[keys[i]].isImposter) imposterCount++;
                else citizenCount++;
            }

            if(imposterCount >= citizenCount) {
                //임포승
                broadcast(socket,JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList,gameOverCase:0})}),true);
                room.initRoom();
                return;
            }

        }
        //아무도 표를 받지 않았거나 동표임
        
        if(room.isEnd) {
            room.changeTime();
        }
        else{
            room.socketList.forEach(soc => {
                soc.send(JSON.stringify({type:"VOTE_TIME_END",payload:""}));
            });

            room.startTimer();
        }
    }
}

function deadReportOrEmergency(socket,type) {

    let room = roomList[socket.room];
    let keys = Object.keys(room.userList);

    let posList = SetSpawnPoint(keys.length);

    for(let i = 0; i < keys.length; i++) {
        userList[keys[i]].position = posList[i];
    }

    room.startVoteTimer();
    room.isEnd = false;

    let dataList = Object.values(room.userList);

    broadcast(socket,JSON.stringify({type:"VOTE_TIME",payload:JSON.stringify({dataList,type})}));
}

function broadcast(socket,msg,isEnd = false) {
    let room = roomList[socket.room];

    if(room === undefined) return;

    room.socketList.forEach(soc => { 
        if(isEnd) soc.state = SocketState.IN_ROOM;
        soc.send(msg);
    });
}

function sendError(msg, socket) //에러 보내기용 함수
{
    socket.send(JSON.stringify({type:"ERROR", payload:msg}));
}

function exitRoom(socket, roomNum) //방에서 나갔을 때의 처리
{
    let room = roomList[roomNum]; //해당 방 받아오기

    socket.room = 0; //나왔으니 룸 초기화

    socket.state = SocketState.IN_LOBBY; //방에서 나왔으니 state 바꿔주고
    room.curUserNum--; //그 방의 인원수--;
    room.removeSocket(socket.id);

    if(userList[socket.id].master && room.curUserNum > 0) { //마스터가 나갔을때 방장권한을 넘겨주기
        let keys = Object.keys(room.userList);
        userList[keys[0]].master = true;
    }

    if(userList[socket.id] !== undefined){ 
        // 초기화
        
        userList[socket.id].roomNum = 0;
        userList[socket.id].master = false; 
        userList[socket.id].isImposter = false;
        userList[socket.id].isDie = false;
        userList[socket.id].voteNum = 0;
        userList[socket.id].voteComplete = false;
    }
    
    
    if(room.curUserNum <= 0){ //사람이 0명일때 room delete
        //console.log(roomList);
        room.stopTimer();
        delete roomList[roomNum];
        //console.log(roomList);
        return;
    }

    room.socketList.forEach(soc => {
        if(soc.id === socket.id) return;
        soc.send(JSON.stringify({type:"DISCONNECT",payload:socket.id}))
    });
}

function refreshRoom(socket) //룸정보 갱신
{
    //let keys = Object.keys(roomList); //roomList의 키들을 받아오고
    let value = Object.values(roomList);
    let dataList = [];
    for(let i = 0; i < value.length; i++) {
        dataList.push(value[i].returnData());
    }
    socket.send(JSON.stringify({type:"REFRESH_ROOM", payload:JSON.stringify({dataList})})); 
}

//나중에는 type도 변수로 받아서 처리해 줘야 분리가 된다.
function roomBroadcast(room,sendType = "REFRESH_MASTER") {
    if(room === undefined) return;

    let dataList = Object.values(room.userList); // 전송할 배열
    room.socketList.forEach(soc => {
        soc.send(JSON.stringify({type:sendType,payload:JSON.stringify({dataList})}));
    });
}

function allRoomBroadcast(roomList) {
    let keys = Object.keys(roomList);

    for(let i = 0; i < keys.length; i++) {
        let room = roomList[keys[i]];
        let dataList = Object.values(room.userList);
        //console.log(JSON.stringify({dataList}));
        room.socketList.forEach(soc => {
            soc.send(JSON.stringify({type:"REFRESH_USER",payload:JSON.stringify({dataList})}));
        });
    }
}

let ms200Timer = setInterval(() => {
    allRoomBroadcast(roomList);
},200);