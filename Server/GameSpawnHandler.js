const Vector2 = require("./Vector2.js");

let spawnTrm = new Vector2(2.4,11.4);

let spawnDistance = 0.9;

let spawnPoint = {};

function SetSpawnPoint(userLength) {
    for(let i = 0; i < userLength; i++) {
        let radian = (2.0 * Math.PI) / userLength;
        radian *= i;
        spawnPoint[i] = new Vector2((Math.cos(radian) * spawnDistance) + spawnTrm.x, (Math.sin(radian) * spawnDistance) + spawnTrm.y);
    }

    return Object.values(spawnPoint);
}

module.exports = SetSpawnPoint;