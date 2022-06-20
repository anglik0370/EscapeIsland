using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SO/SkillSO", fileName = "New SkillSO")]
public class SkillSO : ScriptableObject
{
    public int id; //���� id
    public string skillName; //��ų �̸�

    public float coolTime = 10f; //��Ÿ��
    public float timer = 10f; //��Ÿ�� ����ϴ� Ÿ�̸�

    public bool IsCoolTime => timer > 0; //���� ��Ÿ������

    public Action Callback; //��ų �Լ�

    public bool isPassive; //�нú�����

    private void OnEnable()
    {
        InitTimer();
    }

    private void OnDisable()
    {
        InitTimer();
    }

    public void InitTimer()
    {
        timer = 0;
    }

    public void UpdateTimer() //Update���� �������ָ� ��
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
