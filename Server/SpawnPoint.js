const Vector2 = require("./Vector2.js");

let radius = 3;
let spawnTrm = new Vector2(2.4,11.4);

function makeRandom(min,max) {
    return ((Math.random() * (max-min+1)) + min);
}

function GetRandomPos() {
    let x = makeRandom(-radius + spawnTrm.x,radius + spawnTrm.x);
    let d_y = Math.sqrt(Math.pow(radius,2) - Math.pow(x - spawnTrm.x,2)); //(9 - (5.76~9))^2
    if(d_y > 0) {
        d_y *= Math.floor(Math.random() * 2) === 0 ? -1 : 1;
    }
    let y = spawnTrm.y + d_y;

    console.log(`x = ${x}, y = ${y}`);
    return new Vector2(x,y);
}

module.exports = GetRandomPos;