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

    private List<SkillSO> skillList;

    [SerializeField]
    private const string TRAP_NAME = "�� ��ġ";

    private void Awake()
    {
        skillList = Resources.LoadAll<SkillSO>("SkillSO").ToList();
        skillList.Sort((x, y) => x.id.CompareTo(y.id)); //���̵� ������ ����

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

    private void AmberSkill()
    {
        print($"{skillList[AMBER].skillName} ���");
        PlayerManager.Instance.AccelerationPlayer(AMBER);
    }

    private void CherrySkill()
    {
        print($"{skillList[CHERRY].skillName} ���");
    }

    private void IanSkill()
    {
        print($"{skillList[IAN].skillName} ���");
    }

    private void JosuhaSkill()
    {
        print($"{skillList[JOSUHA].skillName} ���");
    }

    private void RaiSkill()
    {
        print($"{skillList[RAI].skillName} ���");
    }

    private void RandySkill()
    {
        print($"{skillList[RANDY].skillName} ���");
        SendManager.Instance.Send("DISS_RAP");
    }

    private void SarsuSkill()
    {
        print($"{skillList[SARSU].skillName} ���");
    }

    private void WonsongSkill()
    {
        print($"{skillList[WONSONG].skillName} ���");
        PlayerManager.Instance.Inventory.CreateInventory(9);
    }

    private void AnderSkill()
    {
        print($"{skillList[ANDER].skillName} ���");
        SendManager.Instance.SendSabotage(PlayerManager.Instance.Player.socketId, false, TRAP_NAME, null);
    }
}
