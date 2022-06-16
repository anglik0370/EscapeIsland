using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO/SkillSO", fileName = "New SkillSO")]
public class SkillSO : ScriptableObject
{
    public int id; //���� id
    public string skillName; //��ų �̸�

    public float coolTime; //��Ÿ��
    public float timer; //��Ÿ�� ����ϴ� Ÿ�̸�

    public bool IsCoolTime => timer > 0; //���� ��Ÿ������

    public UnityEvent Callback; //��ų �Լ�

    public void InitTimer()
    {
        timer = coolTime;
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
