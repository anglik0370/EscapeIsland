const WebSocket = require('ws');
const port = 31012;

const SocketState = require('./SocketState.js');
const Vector2 = require('./Vector2.js');
const Room = require('./Room.js');
const InGameTimer = require('./InGameTimer.js');
const LoginHandler = require('./LoginHandler.js');
const getWaitingPoint = require('./SpawnPoint.js');
const SetSpawnPoint = require('./GameSpawnHandler.js');

let socketIdx = 0;
let roomIdx = 1; 

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
                    let payload = JSON.parse(data.payload);
                    if(payload.name === ""){
                        sendError("이름을 입력해주세요", socket);
                        return;
                    }
                    let userData = LoginHandler(data.payload,socket);
                    userData.position = Vector2.zero;
                    userData.isImposter = false;
                    userData.isDie = false;
                    userData.voteNum = 0;
                    userData.voteComplete = false;


                    userList[socket.id] = userData;
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
                    if(socket.state !== SocketState.IN_LOBBY){
                        sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
                        return;
                    }

                    let roomInfo = JSON.parse(data.payload);
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
                        userList[socket.id].position = getWaitingPoint();
                    }

                    r.addSocket(socket, userList[socket.id]);
                    roomList[roomIdx] = r;

                    socket.send(JSON.stringify({type:"ENTER_ROOM"}));

                    wsService.clients.forEach(soc=>{
                        if(soc.state != SocketState.IN_LOBBY) 
                            return;
                        refreshRoom(soc);
                    });

                    roomIdx++;
                    break;
                case "JOIN_ROOM":
                    if(socket.state !== SocketState.IN_LOBBY){
                        sendError("로비가 아닌 곳에서 시도를 하였습니다.", socket);
                        return;
                    }

                    let roomNum = JSON.parse(data.payload).roomNum;
                    let targetRoom = roomList[roomNum];

                    if(targetRoom === undefined || targetRoom.curUserNum >= targetRoom.userNum || targetRoom.playing) {
                        sendError("들어갈 수 없는 방입니다.");
                    }
                    socket.room = roomNum;

                    if(userList[socket.id] !== undefined) {
                        userList[socket.id].roomNum = roomNum;
                        userList[socket.id].master = false;
                        userList[socket.id].position = getWaitingPoint();
                    }
                    socket.state = SocketState.IN_ROOM;
                    targetRoom.curUserNum++;
                    targetRoom.addSocket(socket,userList[socket.id]);

                    socket.send(JSON.stringify({type:"ENTER_ROOM"}));

                    wsService.clients.forEach(soc => {
                        if(soc.state === SocketState.IN_LOBBY) {
                            refreshRoom(soc);
                        }
                    });
                    break;
                case "EXIT_ROOM":
                    if(socket.state !== SocketState.IN_ROOM && socket.state !== SocketState.IN_PLAYING){
                        sendError("잘못된 접근입니다.", socket);
                        return;
                    }
                    let eRoomNum = JSON.parse(data.payload).roomNum;

                    exitRoom(socket,eRoomNum);

                    socket.send(JSON.stringify({type:"EXIT_ROOM"}));

                    wsService.clients.forEach(soc=>{
                        if(soc.room === eRoomNum) { //소켓이 해제된 유저가 있었던 방에 있다면
                            roomBroadcast(roomList[eRoomNum]);
                        }
                        if(soc.state === SocketState.IN_LOBBY) {
                            refreshRoom(soc);
                        }
                    });
                    break;
                case "GameStart":
                    if(socket.state !== SocketState.IN_ROOM){
                        sendError("방이 아닌 곳에서 시도를 하였습니다.", socket);
                        return;
                    }

                    let gRoomNum = JSON.parse(data.payload).roomNum;
                    let gTargetRoom = roomList[gRoomNum];

                    if(gTargetRoom.curUserNum < 2) {
                        sendError("최소 2명 이상의 인원이 있어야 합니다.",socket);
                        return;
                    }

                    if(gTargetRoom.curUserNum <= gTargetRoom.kidnapperNum) {
                        sendError("현재 유저의 수가 납치자의 수보다 같거나 적습니다.",socket);
                        return;
                    }

                    let keys = Object.keys(gTargetRoom.userList);
                    let imposterLength = gTargetRoom.kidnapperNum;
                    let idx;

                    for(let i = 0; i < imposterLength; i++) {
                        do {
                            idx = Math.floor(Math.random() * keys.length);
                        }while(userList[keys[idx]].isImposter)

                        userList[keys[idx]].isImposter = true;
                    }

                    roomBroadcast(gTargetRoom);

                    //룸에 있는 플레이어들의 포지션 조정

                    let posList = SetSpawnPoint(keys.length);

                    for(let i = 0; i < keys.length; i++) {
                        userList[keys[i]].position = posList[i];
                    }
                    
                    let dataList = Object.values(gTargetRoom.userList);

                    gTargetRoom.playing = true;
                    gTargetRoom.startTimer();
                    gTargetRoom.socketList.forEach(soc => {
                        soc.state = SocketState.IN_PLAYING;
                        //connectedSocket[soc.id].state = SocketState.IN_PLAYING;
                        soc.send(JSON.stringify({type:"GAME_START",payload:JSON.stringify({dataList})}));
                    });

                    break;
                case "KILL":

                    let dRoomNum = socket.room;
                    let dRoom = roomList[dRoomNum];
                    let socId = JSON.parse(data.payload).targetSocketId;

                    if(userList[socId] !== undefined) {
                        userList[socId].isDie = true;
                    }

                    roomBroadcast(dRoom,"KILL");

                    //여기서 추가로 게임 승패 여부도 검사해야 할 듯?

                    // let imposterCount = 0;
                    // let citizenCount = 0;

                    // let dRoomUserList = Object.values(dRoom.userList);

                    // dRoomUserList.forEach(user => {
                    //     if(user.isDie) return;

                    //     if(user.isImposter) imposterCount++;
                    //     else citizenCount++;
                    // });

                    // if(imposterCount >= citizenCount) {
                    //     //임포승
                    //     dRoom.socketList.forEach(soc => {
                    //         soc.send(JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList:dRoomUserList})}));
                    //     });
                    //     dRoom.initRoom();
                    // }

                    
                    break;
                case "GET_ITEM":
                    let spawnerId = JSON.parse(data.payload).spawnerId;
                    let getItemRoom = roomList[socket.room];

                    getItemRoom.socketList.forEach(soc => {
                        if(soc.id == socket.id) return;
                        soc.send(JSON.stringify({type:"GET_ITEM",payload:spawnerId}));
                    });
                    break;
                case "STORAGE_DROP":
                    let itemSOId = JSON.parse(data.payload).itemSOId;
                    let getItemR = roomList[socket.room];
                    getItemR.socketList.forEach(soc => {
                        if(soc.id == socket.id) return;
                        soc.send(JSON.stringify({type:"STORAGE_DROP",payload:itemSOId}));
                    });
                    break;
                case "STORAGE_FULL":
                    let fullRoom = roomList[socket.room];

                    if(fullRoom.inGameTimer.isLightTime) {
                        fullRoom.inGameTimer.isEndGame = true;
                        fullRoom.socketList.forEach(soc => {
                            soc.send(JSON.stringify({type:"STORAGE_FULL",payload:"저녁까지 쳐 버티도록 하세요"}));
                        });
                    }
                    else {
                        let dataList = Object.values(fullRoom.userList);

                        fullRoom.socketList.forEach(soc => {
                            soc.send(JSON.stringify({type:"WIN_CITIZEN",payload:JSON.stringify({dataList})}));
                        });

                        fullRoom.initRoom();
                    }

                    
                    break;
                case "START_REFINERY":
                    let startData = JSON.parse(data.payload);
                    let startRefineryId = startData.refineryId;
                    let startItemSOId = startData.itemSOId;

                    let startRoom = roomList[socket.room];

                    startRoom.socketList.forEach(soc => {
                        soc.send(JSON.stringify({type:"START_REFINERY",payload:JSON.stringify({refineryId:startRefineryId,itemSOId:startItemSOId})}));
                    });

                    break;
                case "RESET_REFINERY":
                    let resetRefineryId = JSON.parse(data.payload).refineryId;

                    let resetRoom = roomList[socket.room];

                    resetRoom.socketList.forEach(soc => {
                        soc.send(JSON.stringify({type:"RESET_REFINERY",payload:resetRefineryId}));
                    });
                    break;
                case "TAKE_REFINERY":
                    let takeRefineryId = JSON.parse(data.payload).refineryId;
                    let takeRoom = roomList[socket.room];

                    takeRoom.socketList.forEach(soc => {
                        soc.send(JSON.stringify({type:"TAKE_REFINERY",payload:takeRefineryId}));
                    });
                    break;
                case "CHAT":
                    let chatRoom = roomList[socket.room];

                    chatRoom.socketList.forEach(soc => {
                        soc.send(JSON.stringify({type:"CHAT",payload:data.payload}));
                    });

                    break;
                case "VOTE_COMPLETE":
                    let completePayload = JSON.parse(data.payload);

                    let completeRoom = roomList[socket.room];

                    if(completePayload.voteTargetId === -1){

                    }
                    else {
                        userList[completePayload.voteTargetId].voteNum++;
                    }

                    userList[completePayload.voterId].voteComplete = true;

                    completeRoom.socketList.forEach(soc => {
                        soc.send(JSON.stringify({type:"VOTE_COMPLETE",payload:JSON.stringify({voterId:completePayload.voterId,voteTargetId:completePayload.voteTargetId})}))
                    });

                    let comRoomKeys = Object.keys(completeRoom.userList);
                    let allComplete = true;
                    let targetSocIdArr = [];

                    for(let i = 0; i < comRoomKeys.length; i++) {
                        //안죽었을때 & 투표완료했을때 넘어가야함
                        if((!userList[comRoomKeys[i]].voteComplete && !userList[comRoomKeys[i]].isDie)) {
                            allComplete = false;
                            break;
                        }
                    }
                    
                    if(allComplete) {
                        let dummy = 0;

                        for(let i = 0; i < comRoomKeys.length; i++) {
                            if(dummy != 0 && userList[comRoomKeys[i]].voteNum == dummy) {
                                targetSocIdArr.push(userList[comRoomKeys[i]].socketId);
                            }
                            else if(userList[comRoomKeys[i]].voteNum > dummy) {
                                dummy = userList[comRoomKeys[i]].voteNum;
                                targetSocIdArr.length = 0;
                                targetSocIdArr.push(userList[comRoomKeys[i]].socketId);
                            }

                            userList[comRoomKeys[i]].voteNum = 0;
                            userList[comRoomKeys[i]].voteComplete = false;
                        }
                        
                        if(targetSocIdArr.length == 1) {
                            completeRoom.socketList.forEach(soc => {
                                soc.send(JSON.stringify({type:"VOTE_DIE",payload:targetSocIdArr[0]}));
                            });
                            userList[targetSocIdArr[0]].isDie = true;
                            return;
                        }
                        //아무도 표를 받지 않았거나 동표임
                        
                        if(completeRoom.isEnd) {
                            completeRoom.changeTime();
                        }
                        else{
                            completeRoom.socketList.forEach(soc => {
                                soc.send(JSON.stringify({type:"VOTE_TIME_END",payload:""}));
                            });
            
                            completeRoom.startTimer();
                        }
                    }

                    break;
                case "EMERGENCY":
                    let emRoom = roomList[socket.room];
                    let emKeys = Object.keys(emRoom.userList);
                    let emPosList = SetSpawnPoint(emKeys.length);

                    for(let i = 0; i < emKeys.length; i++) {
                        userList[emKeys[i]].position = emPosList[i];
                    }

                    emRoom.startVoteTimer();
                    emRoom.isEnd = false;

                    let emDataList = Object.values(emRoom.userList);

                    emRoom.socketList.forEach(soc => {
                        soc.send(JSON.stringify({type:"VOTE_TIME",payload:JSON.stringify({dataList:emDataList})}));
                    });
                    break;
                case "DEAD_REPORT":
                    let drRoom = roomList[socket.room];
                    let drKeys = Object.keys(drRoom.userList);
                    let drPosList = SetSpawnPoint(drKeys.length);

                    for(let i = 0; i < drKeys.length; i++) {
                        userList[drKeys[i]].position = drPosList[i];
                    }

                    drRoom.startVoteTimer();
                    drRoom.isEnd = false;

                    let drDataList = Object.values(drRoom.userList);

                    drRoom.socketList.forEach(soc => {
                        soc.send(JSON.stringify({type:"VOTE_TIME",payload:JSON.stringify({dataList:drDataList})}));
                    });
                    break;
            }
        }
        catch (error) {
            console.log(`잘못된 요청 발생 : ${msg}`);
            console.log(error);
        }
    });

});

function sendError(msg, socket) //에러 보내기용 함수
{
    socket.send(JSON.stringify({type:"ERROR", payload:msg}));
}

function exitRoom(socket, roomNum) //방에서 나갔을 때의 처리
{
    let targetRoom = roomList[roomNum]; //해당 방 받아오기

    socket.room = 0; //나왔으니 룸 초기화

    if(userList[socket.id] !== undefined){ 
        // 초기화
        
        userList[socket.id].roomNum = 0;
        userList[socket.id].master = false; 
        userList[socket.id].isImposter = false;
        userList[socket.id].isDie = false;
        userList[socket.id].voteNum = 0;
        userList[socket.id].voteComplete = false;
    }

    socket.state = SocketState.IN_LOBBY; //방에서 나왔으니 state 바꿔주고
    targetRoom.curUserNum--; //그 방의 인원수--;
    targetRoom.removeSocket(socket.id);

    if(userList[socket.id].master && targetRoom.curUserNum > 0) { //마스터가 나갔을때 방장권한을 넘겨주기
        let keys = Object.keys(targetRoom.userList);
        userList[keys[0]].master = true;
    }
    
    
    if(targetRoom.curUserNum <= 0){ //사람이 0명일때 room delete
        //console.log(roomList);
        targetRoom.deleteRoom();
        delete roomList[roomNum];
        //console.log(roomList);
        return;
    }

    targetRoom.socketList.forEach(soc => {
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
        let targetRoom = roomList[keys[i]];
        let dataList = Object.values(targetRoom.userList);

        targetRoom.socketList.forEach(soc => {
            soc.send(JSON.stringify({type:"REFRESH_USER",payload:JSON.stringify({dataList})}));
        });
    }
}

let ms200Timer = setInterval(() => {
    allRoomBroadcast(roomList);
},200);