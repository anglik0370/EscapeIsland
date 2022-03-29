const regex = /^[ㄱ-ㅎ|가-힣|a-z|A-Z|0-9|]{1,15}$/;        

function getRegex() {
    return regex;
}

module.exports = getRegex;