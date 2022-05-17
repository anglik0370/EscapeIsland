const roomRegex = /^[ㄱ-ㅎ|가-힣|a-z|A-Z|0-9|\s]{1,15}$/;        
const nameRegex = /^[ㄱ-ㅎ|가-힣|a-z|A-Z|0-9|]{1,15}$/;       

function getRoomRegex() {
    return roomRegex;
}

function getNameRegex() {
    return nameRegex;
}

module.exports.roomRegex = getRoomRegex;
module.exports.nameRegex = getNameRegex;