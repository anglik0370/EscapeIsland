using System.Collections.Generic;
[System.Serializable]
public class RoomListVO : VO
{
    public List<RoomVO> dataList;

    public RoomListVO()
    {

    }

    public RoomListVO(List<RoomVO> list)
    {
        dataList = list;
    }
}
