using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    public static LogPanel Instance { get; private set; }

    private ScrollRect scrollRect;

    [SerializeField]
    private Transform contentTrm;

    [SerializeField]
    private LogText logTextPrefab;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        scrollRect = GetComponent<ScrollRect>();
    }

    public void CreateLogText(string str)
    {
        LogText logText = Instantiate(logTextPrefab, contentTrm);
        logText.SetText(str);

        StartCoroutine(CoroutineHandler.EndFrame(() => scrollRect.verticalNormalizedPosition = 0f));
    }

    public void GlobalSkillLog(Player caster, string skillName)
    {
        string str = $"[<color={caster.CurTeam.ToString().ToLower()}>{caster.socketName}</color>] {skillName} 사용";
        CreateLogText(str);
    }

    public void SingleSkillLog(Player caster, Player victim, string skillName)
    {
        string str = $"[<color={caster.CurTeam.ToString().ToLower()}>{caster.socketName}</color>] <color={victim.CurTeam.ToString().ToLower()}>{victim.socketName}</color> 에게 {skillName} 사용";
        CreateLogText(str);
    }
}
