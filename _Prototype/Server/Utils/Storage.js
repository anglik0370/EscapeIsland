class Storage {
    constructor() {
        this.maxAmountItemList = {};
        this.curAmountItemList = {};

        this.totalNeedItemAmount = 0;
        this.totalCollectedItemAmount = 0;
    }

    initStorage() {
        for(let key in this.curAmountItemList) {
            this.curAmountItemList[key] = 0;
        }

        this.totalCollectedItemAmount = 0;
    }

    addItemAmount(itemId) {
        this.curAmountItemList[itemId]++;
        this.totalCollectedItemAmount++;

        return this.maxAmountItemList[itemId] <= this.curAmountItemList[itemId];
    }

    removeItemAmount(itemId) {
        this.curAmountItemList[itemId]--;
        this.totalCollectedItemAmount--;
    }

    setItemAmount(itemId,amount) {
        this.maxAmountItemList[itemId] = amount;
        this.curAmountItemList[itemId] = 0;

        this.setTotalAmount();
    }

    setTotalAmount() {
        this.totalNeedItemAmount = 0;

        for(let key in this.maxAmountItemList) {
            this.totalNeedItemAmount += this.maxAmountItemList[key];
        }
    }

    IsFullStorage() {
        return this.totalNeedItemAmount <= this.totalCollectedItemAmount;
    }
}

module.exports = Storage;