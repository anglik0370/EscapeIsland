const WebSocket = require('ws');
const port = 31012;

const SocketState = require('./SocketState.js');
const Vector2 = require('./Vector2.js');
const LoginHandler = require('./LoginHandler.js');

let socketIdx = 0;

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
                    refreshUser(soc,roomNum); 
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
                    userList[socket.id] = userData;
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
    targetRoom.number--; //그 방의 인원수--;

    if(targetRoom.number === 0){ //사람이 0명일때 room delete
        delete roomList[roomNum];
    }
    else if(targetRoom.number === 1){ //1명만 있을때 그 1명에게 방장권한을 넘김
        wsService.clients.forEach(soc=>{
            if(soc.room === roomNum)
                userList[soc.id].master = true;
        })
    }
    userList[socket.id].master = false; //나간 유저의 방장권한은 해제해준다.
}

function refreshRoom(socket) //룸정보 갱신
{
    let keys = Object.keys(roomList); //roomList의 키들을 받아오고
    let dataList = []; // 전송할 배열
    for(let i=0; i<keys.length; i++){
        dataList.push(roomList[keys[i]]); //현재 존재하는 룸들의 정보를 푸시해준다.
    }
    socket.send(JSON.stringify({type:"REFRESH_ROOM", payload:JSON.stringify({dataList})})); 
}
function refreshUser(socket, roomNum) //유저정보 갱신
{
    let keys = Object.keys(userList); //userList의 키들을 받아오고
    let dataList = []; // 전송할 배열
    for(let i=0; i<keys.length; i++){
        if(userList[keys[i]].roomNum === roomNum){ //유저중 roomNum에 있는 유저들이라면
            dataList.push(userList[keys[i]]); //푸시
        }
    }
    //console.log(userList);
    socket.send(JSON.stringify({type:"REFRESH_USER", payload:JSON.stringify({dataList})}));
}