using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    private WaitForSeconds trapTime;

    public Player enterPlayer = null;

    bool isOnce = false;
    bool isTrap = false;

    public int id;

    private void Start()
    {
        EventManager.SubGameOver(goc => Init());
        EventManager.SubExitRoom(Init);

        trapTime = new WaitForSeconds(15f);
    }

    void OnEnable()
    {
        if(!isOnce)
        {
            isOnce = true;
            return;
        }

        StartCoroutine(SetDisable());
    }

    private void Init()
    {
        isTrap = false;
        gameObject.SetActive(false);

        enterPlayer.isTrap = false;
        enterPlayer = null;
    }

    IEnumerator SetDisable()
    {
        sr.enabled = true;
        if (NetworkManager.instance.User.isKidnapper) yield break;

        yield return CoroutineHandler.oneSec;
        sr.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Player p = col.GetComponentInParent<Player>();
            
            if(p != null && !p.isKidnapper && !isTrap)
            {
                enterPlayer = p;
                SendManager.Instance.SendTrap(id);
            }
        }
    }

    public void EnterTrap()
    {
        StartCoroutine(EnterTrapCo());
    }

    IEnumerator EnterTrapCo()
    {
        isTrap = sr.enabled = enterPlayer.isTrap = true;

        yield return trapTime;

        Init();
    }
}
