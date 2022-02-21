[System.Serializable]
public class VoteCompleteVO
{
    public int voterId;
    public int voteTargetId;

    public VoteCompleteVO()
    {

    }

    public VoteCompleteVO(int voterId,int voteTargetId)
    {
        this.voterId = voterId;
        this.voteTargetId = voteTargetId;
    }
}
