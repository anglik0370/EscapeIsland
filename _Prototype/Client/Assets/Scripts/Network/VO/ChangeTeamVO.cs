using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChangeTeamVO 
{
    public UserVO user;
    public List<int> redSelectedCharId;
    public List<int> blueSelectedCharId;
}
