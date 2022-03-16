const WebSocket = require('ws');
const port = 31012;

const {Rooms} = require('./Rooms.js');
const SocketState = require('./SocketState.js');
const Vector2 = require('./Vector2.js');
//const Room = require('./Room.js');
const InGameTimer = require('./InGameTimer.js');
const LoginHandler = require('./LoginHandler.js');
const GetRandomPos = require('./SpawnPoint.js');
const SetSpawnPoint = require('./GameSpawnHandler.js');
const sendError = require('./SendError.js');
const _ = require('lodash');

let socketIdx = 0;

let testIdx = 1000;

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
    socket.server = wsService;

    socket.on("close", () => {
        console.log(`소켓 연결 해제 id: ${socket.id}`);
        let roomNum = socket.room; //현재 소켓의 룸 idx를 받아옴

        //현재 socket이 룸에 들어가 있다면
        if(socket.room > 0 && socket.state === SocketState.IN_ROOM) {
            Rooms.exit(socket,roomNum); //방 나가기
            wsService.clients.forEach(soc => {
                if(soc.room === roomNum) { //소켓이 해제된 유저가 있었던 방에 있다면
                    //refreshUser(soc,roomNum); 
                    Rooms.roomBroadcast(roomNum);
                }
                if(soc.state === SocketState.IN_LOBBY) {
                    Rooms.refreshRoom(soc);
                }
            });
        }
        //현재 socket의 state가 IN_PLAYING일때
        if(socket.state === SocketState.IN_PLAYING) {
            if(socket.room > 0) {
                Rooms.exit(socket,roomNum);
            }
            wsService.clients.forEach(soc=>{
                if(soc.state === SocketState.IN_LOBBY) {
                    Rooms.refreshRoom(soc);
                }
                if(soc.room === roomNum) {
                    Rooms.roomBroadcast(roomNum);
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
                    Rooms.refreshRoom(socket);
                    break;
                case "CREATE_ROOM":
                    Rooms.createRoom(socket,JSON.parse(data.payload),userList[socket.id]);
                    break;
                case "JOIN_ROOM":
                    Rooms.joinRoom(socket,userList[socket.id],JSON.parse(data.payload).roomNum);
                    break;
                case "TEST_CLIENT":
                    //userList[socket.id].isImposter = true;
                    testClient(socket);
                    break;
                case "EXIT_ROOM":
                    Rooms.exitRoom(socket,JSON.parse(data.payload).roomNum);
                    break;
                case "GameStart":
                    Rooms.getRoom(JSON.parse(data.payload).roomNum).gameStart(socket);
                    break;
                case "KILL":
                    kill(socket,JSON.parse(data.payload));
                    break;
                case "GET_ITEM":
                    let spawnerId = JSON.parse(data.payload).spawnerId;

                   Rooms.getRoom(socket.room).broadcast(socket,JSON.stringify({type:"GET_ITEM",payload:spawnerId}));
                    break;
                case "STORAGE_DROP":
                    let itemSOId = JSON.parse(data.payload).itemSOId;

                    Rooms.getRoom(socket.room).broadcast(socket,JSON.stringify({type:"STORAGE_DROP",payload:itemSOId}));
                    break;
                case "STORAGE_FULL":
                    storageFull(socket);
                    break;
                case "START_REFINERY":
                    let startData = JSON.parse(data.payload);

                    Rooms.getRoom(socket.room).broadcast(socket,JSON.stringify({type:"START_REFINERY",payload:JSON.stringify({refineryId:startData.refineryId,itemSOId:startData.itemSOId})}))

                    break;
                case "RESET_REFINERY":
                    let resetRefineryId = JSON.parse(data.payload).refineryId;

                    Rooms.getRoom(socket.room).broadcast(socket,JSON.stringify({type:"RESET_REFINERY",payload:resetRefineryId}));
                    break;
                case "TAKE_REFINERY":
                    let takeRefineryId = JSON.parse(data.payload).refineryId;

                    Rooms.getRoom(socket.room).broadcast(socket,JSON.stringify({type:"TAKE_REFINERY",payload:takeRefineryId}));
                    break;
                case "CHAT":
                    Rooms.getRoom(socket.room).broadcast(socket,JSON.stringify({type:"CHAT",payload:data.payload}))
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

function testClient(socket) {
    // if(userList[socket.id] !== undefined)
    //     userList[socket.id].isImposter = true;

    let room = Rooms.getRoom(socket.room);

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

function kill(socket,payload) {
    let room = Rooms.getRoom(socket.room);

    if(room === undefined) return;

    let socId = payload.targetSocketId;

    if(userList[socId] !== undefined) {
        userList[socId].isDie = true;
    }

    Rooms.roomBroadcast(socket.room,"KILL");

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
        room.broadcast(JSON.stringify({type:"WIN_KIDNAPPER",payload:JSON.stringify({dataList,gameOverCase:0})}),true);
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

Rooms.startServer();


// function refreshRoom(socket) //룸정보 갱신
// {
//     //let keys = Object.keys(roomList); //roomList의 키들을 받아오고
//     let value = Object.values(roomList);
//     let dataList = [];
//     for(let i = 0; i < value.length; i++) {
//         dataList.push(value[i].returnData());
//     }
//     socket.send(JSON.stringify({type:"REFRESH_ROOM", payload:JSON.stringify({dataList})})); 
// }

// //나중에는 type도 변수로 받아서 처리해 줘야 분리가 된다.
// function roomBroadcast(room,sendType = "REFRESH_MASTER") {
//     if(room === undefined) return;

//     let dataList = Object.values(room.userList); // 전송할 배열
//     room.socketList.forEach(soc => {
//         soc.send(JSON.stringify({type:sendType,payload:JSON.stringify({dataList})}));
//     });
// }

// function allRoomBroadcast(roomList) {
//     let keys = Object.keys(roomList);

//     for(let i = 0; i < keys.length; i++) {
//         let room = roomList[keys[i]];
//         let dataList = Object.values(room.userList);
//         //console.log(JSON.stringify({dataList}));
//         room.socketList.forEach(soc => {
//             soc.send(JSON.stringify({type:"REFRESH_USER",payload:JSON.stringify({dataList})}));
//         });
//     }
// }

// let ms200Timer = setInterval(() => {
//     allRoomBroadcast(roomList);
// },200);