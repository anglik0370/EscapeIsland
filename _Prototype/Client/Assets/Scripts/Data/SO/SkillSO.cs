using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SO/SkillSO", fileName = "New SkillSO")]
public class SkillSO : ScriptableObject
{
    public int id; //고유 id
    public string skillName; //스킬 이름

    public Sprite skillIcon; //스킬 아이콘
    [TextArea]
    public string skillExplanation; //스킬 설명

    public SkillType skillType; //스킬 타입

    public float coolTime = 10f; //쿨타임

    public AudioClip skillSFX;
    [HideInInspector]
    public float timer = 10f; //쿨타임 계산하는 타이머

    public bool IsCoolTime => timer > 0; //현재 쿨타임인지

    public Action Callback; //스킬 함수

    public bool isPassive; //패시브인지

    protected void OnEnable()
    {
        InitTimer();
    }

    protected void OnDisable()
    {
        InitTimer();
    }

    public void InitTimer()
    {
        timer = 0;
    }

    public virtual void UpdateTimer() //Update에서 실행해주면 됨
    {
        if(IsCoolTime)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                timer = 0;
            }
        }
    }
}
