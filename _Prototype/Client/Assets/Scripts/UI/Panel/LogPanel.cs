using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    public static LogPanel Instance { get; private set; }

    [SerializeField]
    private Queue<LogText> logTexts = new Queue<LogText>();

    private ScrollRect scrollRect;

    [SerializeField]
    private Transform contentTrm;

    [SerializeField]
    private LogText logTextPrefab;

    private bool isGameOver = false;

    private Coroutine co;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        scrollRect = GetComponent<ScrollRect>();
    }
    
    private void Start() 
    {
        EventManager.SubGameOver(goc => 
        {
            isGameOver = true;

            if(co != null) StopCoroutine(co);
        });

        EventManager.SubExitRoom(() => 
        {
            isGameOver = true;

            if(co != null) StopCoroutine(co);
        });

        EventManager.SubGameStart(p => 
        {
            isGameOver = false;
        });
    } 
    public void CreateLogText(string str)
    {
        print(str);

        UIManager.Instance.AlertText(str, AlertType.GameEvent);

        LogText logText = Instantiate(logTextPrefab, contentTrm);
        logText.SetText($"[{TimeHandler.Instance.GetCurTime()}] {str}");

        logTexts.Enqueue(logText);

        co = StartCoroutine(CoroutineHandler.EndFrame(() => scrollRect.verticalNormalizedPosition = 0f));
    }

    public void GlobalSkillLog(Player caster, string skillName)
    {
        string str = $"<color={caster.CurTeam.ToString().ToLower()}>{caster.socketName}</color>��(��) {skillName}��(��) ����Ͽ����ϴ�";
        CreateLogText(str);
    }

    public void SingleSkillLog(Player caster, Player victim, string skillName)
    {
        string str = $"<color={caster.CurTeam.ToString().ToLower()}>{caster.socketName}</color>��(��) <color={victim.CurTeam.ToString().ToLower()}>{victim.socketName}</color> ���� {skillName}��(��) ����Ͽ����ϴ�";
        CreateLogText(str);
    }

    public void OccupationLog(Team team, Area area)
    {
        string teamStr = team == Team.BLUE ? "�����" : "������";

        string areaStr = "";

        switch (area)
        {
            case Area.Beach:
                areaStr = "�غ�";
                break;
            case Area.Cave:
                areaStr = "����";
                break;
            case Area.Field:
                areaStr = "��";
                break;
            case Area.Forest:
                areaStr = "��";
                break;
        }

        if(areaStr.Equals("")){
            Debug.Log("�߸��� ���� �α�");
            return;
        }

        string str = $"<color={team.ToString().ToLower()}>{teamStr}</color>�� {areaStr}��(��) �����Ͽ����ϴ�";
        CreateLogText(str);
    }

    public void StorageFullLog(Team team, ItemSO item)
    {
        string teamStr = team == Team.BLUE ? "�����" : "������";

        string str = $"<color={team.ToString().ToLower()}>{teamStr}</color>�� {item.itemName}��(��) ��� ��ҽ��ϴ�";
        CreateLogText(str);
    }
    
    public void ClearLog()
    {
        foreach (var log in logTexts)
        {
            Destroy(log.gameObject);
        }

        logTexts.Clear();
    }
}
