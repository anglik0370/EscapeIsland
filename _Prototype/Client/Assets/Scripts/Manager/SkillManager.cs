using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private const int SIMON = 9;
    private const int LEON = 10;
    private const int KION = 11;

    private List<SkillSO> skillList;

    [SerializeField]
    private const string TRAP_NAME = "덫 설치";
    [SerializeField]
    private const string ARSON_NAME = "방화";

    [SerializeField]
    private Transform redShipParentTrm;
    [SerializeField]
    private Transform blueShipParentTrm;

    private Player user;

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
        skillList[SIMON].Callback = SimonSkill;
        skillList[LEON].Callback = LeonSkill;
        skillList[KION].Callback = KionSkill;
    }

    private void Start()
    {
        EventManager.SubGameInit(() => trapCount = 0);

        EventManager.SubEnterRoom(p => user = p);

        EventManager.SubGameStart(p =>
        {
            AreaRestrictionSkillSO simonSkill = skillList[SIMON] as AreaRestrictionSkillSO;

            List<ItemStorage> itemStorageList = new List<ItemStorage>();

            simonSkill.colliderList?.Clear();

            if (user.CurTeam == Team.RED)
            {
                itemStorageList = blueShipParentTrm.GetComponentsInChildren<ItemStorage>().ToList();
            }
            else
            {
                itemStorageList = redShipParentTrm.GetComponentsInChildren<ItemStorage>().ToList();
            }

            for (int i = 0; i < itemStorageList.Count; i++)
            {
                simonSkill.colliderList.Add(itemStorageList[i].InteractionCol);
            }
        });
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

        foreach (Player p in playerList)
        {
            if(user.Area.Equals(p.Area)) //같은 구역 안에 있다면
            {
                socketIdList.Add(p.socketId); //버프 적용해
            }
        }

        socketIdList.Add(user.socketId);

        SendManager.Instance.SendSKill(new SkillVO(CharacterType.Cherry,user.socketId, user.CurTeam, socketIdList, skillList[CHERRY].skillName));
    }

    private void IanSkill()
    {
        print($"{skillList[IAN].skillName} 사용");

        SendManager.Instance.SendSKill(new SkillVO(CharacterType.IAN,user.socketId, user.CurTeam, skillList[IAN].skillName));
    }

    private void JosuhaSkill()
    {
        print($"{skillList[JOSUHA].skillName} 사용");

        List<Player> playerList = NetworkManager.instance.GetPlayerList();
        List<int> socketIdList = new List<int>();

        WideAreaSkillSO joshuaSO = (WideAreaSkillSO)skillList[JOSUHA];

        foreach (Player p in playerList)
        {
            if (!p.CurTeam.Equals(user.CurTeam) && Vector2.Distance(user.transform.position, p.transform.position) <= joshuaSO.skillRange)
            {
                socketIdList.Add(p.socketId);
            }
        }

        SendManager.Instance.SendSKill(new SkillVO(CharacterType.Joshua,user.socketId, user.CurTeam, socketIdList, skillList[JOSUHA].skillName));
    }

    private void RaiSkill()
    {
        print($"{skillList[RAI].skillName} 사용");

        TargetingSkillSO raiSO = (TargetingSkillSO)skillList[RAI];

        int targetSocketId = PlayerManager.Instance.GetRangeInPlayerId(raiSO.skillRange);

        if(targetSocketId != 0)
        {
            SendManager.Instance.SendSKill(new SkillVO(CharacterType.Rai, user.socketId, targetSocketId, skillList[RAI].skillName));
        }
    }

    private void RandySkill()
    {
        print($"{skillList[RANDY].skillName} 사용");
        SendManager.Instance.SendSKill(new SkillVO(CharacterType.Randy,user.socketId,PlayerManager.Instance.Player.CurTeam, skillList[RANDY].skillName));
    }

    private void SarsuSkill()
    {
        print($"{skillList[SARSU].skillName} 사용");

        user.inventory.AddItem(MissionPanel.Instance.MissionItemList[Random.Range(0, MissionPanel.Instance.MissionItemList.Count)]);
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

    private void SimonSkill()
    {
        print($"{skillList[SIMON].skillName} 사용");

        AreaRestrictionSkillSO skill = skillList[SIMON] as AreaRestrictionSkillSO;

        Collider2D touchingCol = skill.colliderList.Find(x => Physics2D.IsTouching(x, user.BodyCollider));

        ItemSO item = touchingCol.transform.GetComponentInParent<ItemStorage>().Item;

        ////일단은 랜덤으로 구현
        //ItemSO item = ItemManager.Instance.ItemList[Random.Range(0, ItemManager.Instance.ItemList.Count)];

        Team team = user.CurTeam == Team.RED ? Team.BLUE : Team.RED;

        //여기를 동기화 해줘야됨
        SendManager.Instance.SendSKill(new SkillVO(CharacterType.Simon, user.socketId, item.itemId, team, skillList[SIMON].skillName));
    }

    private void LeonSkill()
    {
        //상대팀만 가져와서
        List<Player> playerList = NetworkManager.instance.GetPlayerList().Where(x => x.CurTeam != user.CurTeam).ToList();
        //랜덤으로 한명 뽑고
        Player targetPlayer = playerList[Random.Range(0, playerList.Count)];

        SendManager.Instance.SendSKill(new SkillVO(CharacterType.Leon, user.socketId, targetPlayer.socketId, skillList[LEON].skillName));
    }

    private void KionSkill()
    {
        print($"{skillList[KION].skillName} 사용");
        SendManager.Instance.SendSKill(new SkillVO(CharacterType.Kion, user.socketId, skillList[KION].skillName, user.CurTeam, user.transform.position));
    }
}