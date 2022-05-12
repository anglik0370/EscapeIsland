
using System.Collections.Generic;

[System.Serializable]

public class SabotageVO
{
    public int starterId;
    public bool isShareCoolTime;
    public string sabotageName;

    public SabotageDataVO data;

    public List<UserVO> userDataList;

    public SabotageVO()
    {

    }

    public SabotageVO(int starterId, bool isShareCoolTime,string sabotageName, SabotageDataVO data)
    {
        this.starterId = starterId;
        this.isShareCoolTime = isShareCoolTime;
        this.sabotageName = sabotageName;
        this.data = data;
        userDataList = null;
    }
}

[System.Serializable]
public class SabotageDataVO
{
    public int arsonId;

    public SabotageDataVO()
    {

    }

    public SabotageDataVO(int arsonId)
    {
        this.arsonId = arsonId;
    }
}