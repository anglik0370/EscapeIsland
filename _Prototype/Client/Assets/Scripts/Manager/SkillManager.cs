using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkillType
{
    Amber = 0,
    Cherry,
    IAN,
    Joshua,
    Rai,
    Randy,
    Sarsu,
    Wonsong,
    Ander
}

public class SkillManager : MonoBehaviour
{
    private const int AMBER = 0;
    private const int CHERRY = 1;
    private const int IAN = 2;
    private const int JOSUHA = 3;
    private const int RAI = 4;
    private const int RANDY = 5;
    private const int SARSU = 6;
    private const int WONSONG = 7;
    private const int ANDER = 8;

    private List<SkillSO> skillList;

    [SerializeField]
    private const string TRAP_NAME = "덫 설치";
    [SerializeField]
    private const string ARSON_NAME = "방화";

    private int trapCount = 0;

    private void Awake()
    {
        skillList = Resources.LoadAll<SkillSO>("SkillSO").ToList();
        skillList.Sort((x, y) => x.id.CompareTo(y.id)); //아이디 순으로 정렬

        skillList[AMBER].Callback = AmberSkill;
        skillList[CHERRY].Callback = CherrySkill;
        skillList[IAN].Callback = IanSkill;
        skillList[JOSUHA].Callback = JosuhaSkill;
        skillList[RAI].Callback = RaiSkill;
        skillList[RANDY].Callback = RandySkill;
        skillList[SARSU].Callback = SarsuSkill;
        skillList[WONSONG].Callback = WonsongSkill;
        skillList[ANDER].Callback = AnderSkill;
    }

    private void Start()
    {
        EventManager.SubGameInit(() => trapCount = 0);
    }

    private void AmberSkill()
    {
        print($"{skillList[AMBER].skillName} 사용");
        PlayerManager.Instance.AccelerationPlayer(AMBER);
    }

    private void CherrySkill()
    {
        print($"{skillList[CHERRY].skillName} 사용");

        List<Player> playerList = NetworkManager.instance.GetPlayerList();
        List<int> socketIdList = new List<int>();

        WideAreaSkillSO cherrySO = (WideAreaSkillSO)skillList[CHERRY];
        Player user = NetworkManager.instance.User;

        foreach (Player p in playerList)
        {
            if(Vector2.Distance(user.transform.position,p.transform.position) <= cherrySO.skillRange)
            {
                socketIdList.Add(p.socketId);
            }
        }

        socketIdList.Add(user.socketId);

        SendManager.Instance.SendSKill(new SkillVO(SkillType.Cherry, user.CurTeam, socketIdList));
    }

    private void IanSkill()
    {
        print($"{skillList[IAN].skillName} 사용");
        //SendManager.Instance.Send("SKILL_IAN");
        SendManager.Instance.SendSKill(new SkillVO(SkillType.IAN, NetworkManager.instance.User.CurTeam));
    }

    private void JosuhaSkill()
    {
        print($"{skillList[JOSUHA].skillName} 사용");

        List<Player> playerList = NetworkManager.instance.GetPlayerList();
        List<int> socketIdList = new List<int>();

        WideAreaSkillSO joshuaSO = (WideAreaSkillSO)skillList[JOSUHA];
        Player user = NetworkManager.instance.User;

        foreach (Player p in playerList)
        {
            if (!p.CurTeam.Equals(user.CurTeam) && Vector2.Distance(user.transform.position, p.transform.position) <= joshuaSO.skillRange)
            {
                socketIdList.Add(p.socketId);
            }
        }

        SendManager.Instance.SendSKill(new SkillVO(SkillType.Joshua, user.CurTeam, socketIdList));
    }

    private void RaiSkill()
    {
        print($"{skillList[RAI].skillName} 사용");

        TargetingSkillSO raiSO = (TargetingSkillSO)skillList[RAI];

        int targetSocketId = PlayerManager.Instance.GetRangeInPlayerId(raiSO.skillRange);

        if(targetSocketId != 0)
        {
            SendManager.Instance.SendSKill(new SkillVO(SkillType.Rai, targetSocketId));
        }
    }

    private void RandySkill()
    {
        print($"{skillList[RANDY].skillName} 사용");
        //SendManager.Instance.Send("DISS_RAP");
        SendManager.Instance.SendSKill(new SkillVO(SkillType.Randy,PlayerManager.Instance.Player.CurTeam));
    }

    private void SarsuSkill()
    {
        print($"{skillList[SARSU].skillName} 사용");
    }

    private void WonsongSkill()
    {
        print($"{skillList[WONSONG].skillName} 사용");
        PlayerManager.Instance.Inventory.CreateInventory(9);
    }

    private void AnderSkill()
    {
        print($"{skillList[ANDER].skillName} 사용");

        //if(trapCount >= 10)
        //{
        //    SendManager.Instance.SendSabotage(PlayerManager.Instance.Player.socketId, ARSON_NAME, PlayerManager.Instance.Player.CurTeam);
        //    trapCount = 0;
        //    return;
        //}

        SendManager.Instance.SendSabotage(PlayerManager.Instance.Player.socketId,TRAP_NAME,PlayerManager.Instance.Player.CurTeam);
        trapCount++;
    }
}
