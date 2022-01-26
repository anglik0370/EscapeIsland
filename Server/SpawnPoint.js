const Vector2 = require("./Vector2.js");

//일단은 임시 좌표
let respawnPoint = [
    new Vector2(0.3,14),
    new Vector2(-1.4,8.5),
    new Vector2(6.3,8.5),
    new Vector2(5,13.5)
];

let waitingPos = respawnPoint[Math.floor(Math.random() * respawnPoint.length)];

module.exports = waitingPos;