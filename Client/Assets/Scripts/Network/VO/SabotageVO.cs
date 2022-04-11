
using System.Collections.Generic;

[System.Serializable]

public class SabotageVO
{
    public bool isShareCoolTime;
    public string sabotageName;

    public SabotageVO()
    {

    }

    public SabotageVO(bool isShareCoolTime,string sabotageName)
    {
        this.isShareCoolTime = isShareCoolTime;
        this.sabotageName = sabotageName;
    }
}