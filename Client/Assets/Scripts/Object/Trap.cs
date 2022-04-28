using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    public Player enterPlayer = null;

    bool isOnce = false;
    bool isTrap = false;

    public int id;

    private void Start()
    {
        EventManager.SubGameOver(goc => Init());
        EventManager.SubExitRoom(Init);

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

        if(enterPlayer != null)
        {
            enterPlayer.isTrap = false;
            enterPlayer = null;
        }
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
            
            if(p != null && !isTrap)
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
        if(enterPlayer != null)
        {
            enterPlayer.isTrap = true;
        }
        isTrap = sr.enabled = true;

        yield return CoroutineHandler.fifteenSec;

        Init();
    }
}
