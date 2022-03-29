using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SabotageButton : MonoBehaviour
{
    [SerializeField]
    private SabotageSO sabotageSO;

    [SerializeField]
    private Button sabotageBtn;
    [SerializeField]
    private Image fillImg;

    private float maxCoolTime;
    private float curCoolTime = 0f;

    private bool canSabotage => sabotageBtn.enabled;

    private Coroutine sabotage = null;

    public void Init(SabotageSO so)
    {
        sabotageSO = so;

        maxCoolTime = so.coolTime;
    }

    private void Start()
    {
        sabotageBtn.onClick.AddListener(StartSabotage);
    }

    public void StartTimer()
    {
        sabotageBtn.enabled = true;
        fillImg.fillAmount = curCoolTime = 0f;

        if (sabotage != null)
        {
            StopCoroutine(sabotage);
        }
        sabotage = StartCoroutine(SabotageTimer());
    }

    IEnumerator SabotageTimer()
    {
        while (true)
        {
            if(!canSabotage)
            {
                curCoolTime -= Time.deltaTime;
                fillImg.fillAmount = curCoolTime / maxCoolTime;
                if(curCoolTime <= 0f)
                {
                    sabotageBtn.enabled = true;
                }
            }
            yield return null;
        }
    }

    private void StartSabotage()
    {
        sabotageBtn.enabled = false;
        curCoolTime = maxCoolTime;

        //해줄것들
    }
}
