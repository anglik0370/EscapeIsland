const Vector2 = require("./Vector2.js");

let redSpawnTrm = new Vector2(16.56,-5.72);
let blueSpawnTrm = new Vector2(-42.25,-5);

let spawnDistance = 3;

function SetSpawnPoint(userLength, isBlue) {
    let spawnPoint = [];
    let spawnTrm = isBlue ? blueSpawnTrm : redSpawnTrm;

    for(let i = 0; i < userLength; i++) {
        let radian = (2.0 * Math.PI) / userLength;
        radian *= i;
        spawnPoint[i] = new Vector2((Math.cos(radian) * spawnDistance) + spawnTrm.x, (Math.sin(radian) * spawnDistance) + spawnTrm.y);
    }

    return spawnPoint;
}

module.exports = SetSpawnPoint;