using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    bool isOnce = false;
    bool isTrap = false;

    public int id;

    public Team team;

    private Coroutine co;

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

        co = StartCoroutine(SetDisable());
    }

    private void Init()
    {
        isTrap = false;
        gameObject.SetActive(false);
    }

    IEnumerator SetDisable()
    {
        sr.enabled = true;
        if (NetworkManager.instance.User.CurTeam.Equals(team)) yield break;

        yield return CoroutineHandler.oneSec;
        sr.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Player p = col.transform.parent.GetComponentInParent<Player>();
            
            if(p != null && !p.IsRemote && !p.CurTeam.Equals(team) &&!isTrap)
            {
                SendManager.Instance.SendTrap(p.socketId, id);
            }
        }
    }

    public void EnterTrap()
    {
        StartCoroutine(EnterTrapCo());
    }

    IEnumerator EnterTrapCo()
    {
        if(co != null)
        {
            StopCoroutine(co);
        }

        sr.enabled = true;
        isTrap = true;

        yield return CoroutineHandler.fifteenSec;

        Init();
    }
}
