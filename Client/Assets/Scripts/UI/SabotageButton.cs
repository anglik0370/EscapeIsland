using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SabotageButton : MonoBehaviour
{
    [SerializeField]
    private SabotageSO sabotageSO;
    public SabotageSO SabotageSO => sabotageSO;

    [SerializeField]
    private Button sabotageBtn;
    [SerializeField]
    private Image sabotageImg;
    [SerializeField]
    private Image fillImg;

    private float maxCoolTime;
    private float curCoolTime = 0f;

    private bool canSabotage => sabotageBtn.enabled;

    private Coroutine sabotage = null;

    public void Init(SabotageSO so)
    {
        sabotageSO = so;
        sabotageImg.sprite = so.sabotageSprite;
        maxCoolTime = so.coolTime;
    }

    private void Start()
    {
        sabotageBtn.onClick.AddListener(SendSabotage);

        EventManager.SubGameStart(p =>
        {
            sabotageBtn.enabled = true;
            fillImg.fillAmount = curCoolTime = 0f;
        });

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

    public void StartSabotage(float coolTime)
    {
        sabotageBtn.enabled = false;
        curCoolTime = maxCoolTime = coolTime;
    }

    public void StartSabotage(SabotageDataVO data)
    {
        if (data != null)
        {
            ArsonManager.Instance.Data = data;
        }
        SabotageSO.callback?.Invoke();
    }

    private void SendSabotage()
    {
        SabotageDataVO data = new SabotageDataVO(ArsonManager.Instance.GetRandomSmelter());

        SendManager.Instance.SendSabotage(NetworkManager.instance.User.socketId,sabotageSO.isShareCoolTime, sabotageSO.sabotageName,data);
    }

    public void SpawnTrap()
    {
        Sabotage.Instance.SpawnTrap();
    }

    public void StartArson()
    {
        ArsonManager.Instance.StartArson();
    }

    public void CloseDoor()
    {
        Sabotage.Instance.CloseDoor();
    }

    public void CantUseRefinery()
    {
        ConvertPanel.Instance.StartCantUseRefinery();
    }
}
