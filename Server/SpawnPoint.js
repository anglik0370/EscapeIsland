const Vector2 = require("./Vector2.js");

//일단은 임시 좌표
let respawnPoint = [
    new Vector2(0,0)
];

let waitingPos = respawnPoint[Math.floor(Math.random() * respawnPoint.length)];

module.exports = waitingPos;