const WebSocket = require('ws');
const port = 31012;

const SocketState = require('./SocketState.js');
const Vector2 = require('./Vector2.js');
const Room = require('./Room.js');
const LoginHandler = require('./LoginHandler.js');
const waitingPos = require('./SpawnPoint.js');

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
            //밑에 코드는 2인용 기준(1ㄷ1게임)이므로 수정해야함.
            //플레이중에 나갔을때 해줘야 할 것 해주기

            // roomBroadcast(JSON.stringify({type:"WIN"}),socket,roomNum);
            // wsService.clients.forEach(soc => {
            //     if(soc.room === roomNum) {
            //         exitRoom(soc, roomNum);
            //     }
            // });
            // if(roomList[roomNum] !== undefined) {
            //     delete roomList[roomNum];
            // }
            // wsService.clients.forEach(soc=>{
            //     if(soc.state !== SocketState.IN_LOBBY) 
            //         return;
            //     refreshRoom(soc);
            // })

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
                    let r = new Room(roomInfo.name,roomIdx,1,roomInfo.userNum,false);
                    socket.state = SocketState.IN_ROOM;
                    socket.room = roomIdx;

                    if(userList[socket.id] !== undefined){
                        userList[socket.id].roomNum = roomIdx;
                        userList[socket.id].master = true;
                        userList[socket.id].position = waitingPos;
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
                        userList[socket.id].position = waitingPos;
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
                case "GameStart":
                    if(socket.state !== SocketState.IN_ROOM){
                        sendError("방이 아닌 곳에서 시도를 하였습니다.", socket);
                        return;
                    }

                    let roomNum = JSON.parse(data.payload).roomNum;
                    let targetRoom = roomList[roomNum];

                    if(targetRoom.curUserNum < 4) {
                        sendError("최소 4명 이상의 인원이 있어야 합니다.",socket);
                        return;
                    }

                    //룸의 인원수의 맞게 임포 수 조정

                    let keys = Object.keys(targetRoom.userList);
                    let imposterLength = keys.length / 4;
                    let idx;

                    for(let i = 0; i < imposterLength; i++) {
                        do {
                            idx = Math.floor(Math.random() * keys.length);
                        }while(targetRoom.userList[keys[idx]].isImposter)

                        targetRoom.userList[keys[idx]].isImposter = true;
                    }

                    //룸에 있는 플레이어들의 포지션 조정
                    

                    targetRoom.playing = true;
                    targetRoom.socketList.forEach(soc => {
                        soc.state = SocketState.IN_PLAYING;
                        soc.send(JSON.stringify({type:"GAME_START"}));
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
        userList[socket.id].roomNum = 0;
    }

    socket.state = SocketState.IN_LOBBY; //방에서 나왔으니 state 바꿔주고
    targetRoom.curUserNum--; //그 방의 인원수--;
    targetRoom.removeSocket(socket.id);

    if(userList[socket.id].master && targetRoom.curUserNum > 0) { //마스터가 나갔을때 방장권한을 넘겨주기
        let keys = Object.keys(targetRoom.userList);
        targetRoom.userList[keys[0]].master = true;
        userList[keys[0]].master = true;
    }
    userList[socket.id].master = false; //나간 유저의 방장권한은 해제해준다.
    
    if(targetRoom.curUserNum === 0){ //사람이 0명일때 room delete
        delete roomList[roomNum];
    }
    // else {
    //     roomBroadcast(targetRoom);
    // }

    
}

function refreshRoom(socket) //룸정보 갱신
{
    let keys = Object.keys(roomList); //roomList의 키들을 받아오고
    let dataList = []; // 전송할 배열
    for(let i=0; i<keys.length; i++){
        let a = roomList[keys[i]];
        let name = a.roomName;
        let roomNum = a.roomNum;
        let curUserNum = a.curUserNum;
        let userNum = a.userNum;
        let playing = a.playing;

        dataList.push({name, roomNum,curUserNum,userNum,playing}); //현재 존재하는 룸들의 정보를 푸시해준다.
    }
    console.log(dataList);
    socket.send(JSON.stringify({type:"REFRESH_ROOM", payload:JSON.stringify({dataList})})); 
}

function roomBroadcast(room) {
    let dataList = Object.values(room.userList); // 전송할 배열
    //console.log(JSON.stringify({type:"REFRESH_MASTER",payload:JSON.stringify({dataList})}));
    room.socketList.forEach(soc => {
        soc.send(JSON.stringify({type:"REFRESH_MASTER",payload:JSON.stringify({dataList})}));
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