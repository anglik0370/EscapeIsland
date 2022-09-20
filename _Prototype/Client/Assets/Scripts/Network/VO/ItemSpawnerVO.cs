[System.Serializable]
public class ItemSpawnerVO : VO
{
    public int spawnerId;
    public int senderId;

    public MissionType missionType;
    public Team team;

    public ItemSpawnerVO(int spawnerId,int senderId, MissionType missionType, Team team)
    {
        this.spawnerId = spawnerId;
        this.senderId = senderId;
        this.missionType = missionType;
        this.team = team;
    }
}