using System.Collections.Generic;

[System.Serializable]
public class RoomVO
{
    public string name;
    public int roomNum;
    public int curUserNum;
    public int userNum;
    public int kidnapperNum;

    public NeedItemVO data;

    public RoomVO()
    {

    }

    public RoomVO(string name,int roomNum,int curUserNum ,int userNum,int kidnapperNum)
    {
        this.name = name;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.kidnapperNum = kidnapperNum;
    }
}

[System.Serializable]
public class NeedItemVO
{
    public List<ItemAmountVO> itemAmountList;

    public NeedItemVO(List<ItemAmountVO> data)
    {
        itemAmountList = data;
    }
}

[System.Serializable]
public class ItemAmountVO
{
    public int itemId;
    public int amount;

    public ItemAmountVO(int id, int amount)
    {
        this.itemId = id;
        this.amount = amount;
    }

    public ItemAmountVO()
    {

    }
}