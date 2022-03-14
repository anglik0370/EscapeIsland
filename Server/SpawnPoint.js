const Vector2 = require("./Vector2.js");

let radius = 3;
let spawnTrm = new Vector2(2.4,11.4);

function makeRandom(min,max) {
    return ((Math.random() * (max-min+1)) + min);
}

function GetRandomPos() {
    let angle = Math.random() * 360;

    let x = Math.cos(angle *  (2 * Math.PI / 360) ) * radius;
    let y = Math.sin(angle *  (2 * Math.PI / 360) ) * radius;

    x+=spawnTrm.x;
    y+=spawnTrm.y;
    
    return new Vector2(x,y);
}

module.exports = GetRandomPos;