
using System.Collections.Generic;

[System.Serializable]

public class SabotageVO
{
    public int starterId;
    public bool isShareCoolTime;
    public string sabotageName;
    public List<UserVO> userDataList;

    public SabotageVO()
    {

    }

    public SabotageVO(int starterId, bool isShareCoolTime,string sabotageName)
    {
        this.starterId = starterId;
        this.isShareCoolTime = isShareCoolTime;
        this.sabotageName = sabotageName;
        userDataList = null;
    }
}