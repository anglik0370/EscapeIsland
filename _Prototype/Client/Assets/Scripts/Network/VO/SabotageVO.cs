
using System.Collections.Generic;

[System.Serializable]

public class SabotageVO : VO
{
    public int starterId;
    public string sabotageName;
    public Team team;

    public List<UserVO> userDataList;

    public SabotageVO()
    {

    }

    public SabotageVO(int starterId,string sabotageName, Team team)
    {
        this.starterId = starterId;
        this.sabotageName = sabotageName;
        this.team = team;
        userDataList = null;
    }
}

//[System.Serializable]
//public class SabotageDataVO
//{
//    public int arsonId;

//    public SabotageDataVO()
//    {

//    }

//    public SabotageDataVO(int arsonId)
//    {
//        this.arsonId = arsonId;
//    }
//}