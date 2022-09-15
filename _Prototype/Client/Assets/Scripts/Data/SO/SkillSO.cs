using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SO/SkillSO", fileName = "New SkillSO")]
public class SkillSO : ScriptableObject
{
    public int id; //���� id
    public string skillName; //��ų �̸�

    public Sprite skillIcon; //��ų ������
    [TextArea]
    public string skillExplanation; //��ų ����

    public SkillType skillType; //��ų Ÿ��

    public float coolTime = 10f; //��Ÿ��

    public AudioClip skillSFX;
    [HideInInspector]
    public float timer = 10f; //��Ÿ�� ����ϴ� Ÿ�̸�

    public bool IsCoolTime => timer > 0; //���� ��Ÿ������

    public Action Callback; //��ų �Լ�

    public bool isPassive; //�нú�����

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

    public virtual void UpdateTimer() //Update���� �������ָ� ��
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
