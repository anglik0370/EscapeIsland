using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeetingVO
{
    public List<UserVO> dataList;
    public int type;

    public MeetingVO()
    {

    }

    public MeetingVO(List<UserVO> list,int type)
    {
        dataList = list;
        this.type = type;
    }
}
