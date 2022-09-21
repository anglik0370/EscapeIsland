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
        string str = $"<color={caster.CurTeam.ToString().ToLower()}>{caster.socketName}</color>이(가) {skillName}을(를) 사용하였습니다";
        CreateLogText(str);
    }

    public void SingleSkillLog(Player caster, Player victim, string skillName)
    {
        string str = $"<color={caster.CurTeam.ToString().ToLower()}>{caster.socketName}</color>이(가) <color={victim.CurTeam.ToString().ToLower()}>{victim.socketName}</color> 에게 {skillName}을(를) 사용하였습니다";
        CreateLogText(str);
    }

    public void OccupationLog(Team team, Area area)
    {
        string teamStr = team == Team.BLUE ? "블루팀" : "레드팀";

        string areaStr = "";

        switch (area)
        {
            case Area.Beach:
                areaStr = "해변";
                break;
            case Area.Cave:
                areaStr = "동굴";
                break;
            case Area.Field:
                areaStr = "밭";
                break;
            case Area.Forest:
                areaStr = "숲";
                break;
        }

        if(areaStr.Equals("")){
            Debug.Log("잘못된 구역 로그");
            return;
        }

        string str = $"<color={team.ToString().ToLower()}>{teamStr}</color>이 {areaStr}을(를) 점령하였습니다";
        CreateLogText(str);
    }

    public void StorageFullLog(Team team, ItemSO item)
    {
        string teamStr = team == Team.BLUE ? "블루팀" : "레드팀";

        string str = $"<color={team.ToString().ToLower()}>{teamStr}</color>이 {item.itemName}을(를) 모두 모았습니다";
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
