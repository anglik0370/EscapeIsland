using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeetingVO
{
    public List<UserVO> dataList;
    public int type;
    public bool isTest;

    public MeetingVO()
    {

    }

    public MeetingVO(List<UserVO> list,int type,bool isTest)
    {
        dataList = list;
        this.type = type;
        this.isTest = isTest;
    }
}
