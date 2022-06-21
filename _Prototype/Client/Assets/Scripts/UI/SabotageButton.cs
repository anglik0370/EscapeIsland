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
    public float CurCoolTime => curCoolTime;

    private bool CanSabotage => sabotageBtn.enabled;
    public bool UseOtherSabotage => SabotagePanel.Instance.SharedSabotage(sabotageSO.sabotageName) 
        && (ArsonManager.Instance.isArson || !ConvertPanel.Instance.EndSabotage());

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
            if(!CanSabotage && !UseOtherSabotage)
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

    public void StartSabotageCoolTime(float coolTime)
    {
        sabotageBtn.enabled = false;
        curCoolTime = maxCoolTime = coolTime;
        fillImg.fillAmount = curCoolTime / maxCoolTime;
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
