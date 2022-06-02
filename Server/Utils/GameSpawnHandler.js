const Vector2 = require("./Vector2.js");

let spawnTrm = new Vector2(1.33,13.21);

let spawnDistance = 3;

let spawnPoint = [];

function SetSpawnPoint(userLength) {
    for(let i = 0; i < userLength; i++) {
        let radian = (2.0 * Math.PI) / userLength;
        radian *= i;
        spawnPoint[i] = new Vector2((Math.cos(radian) * spawnDistance) + spawnTrm.x, (Math.sin(radian) * spawnDistance) + spawnTrm.y);
    }

    return spawnPoint;
}

module.exports = SetSpawnPoint;