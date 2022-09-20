using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private bool isTrap = false;

    public int id;
    public int enterPlayerId;

    public Team team;

    private Coroutine co;

    private void Start()
    {
        EventManager.SubGameOver(goc => Init());
        EventManager.SubExitRoom(Init);

    }

    //void OnEnable()
    //{
    //    if(!isOnce)
    //    {
    //        isOnce = true;
    //        return;
    //    }

    //    co = StartCoroutine(SetDisable());
    //}
    public void SetEnable()
    {
        gameObject.SetActive(true);
        if(co != null)
        {
            StopCoroutine(co);
        }
        co = StartCoroutine(SetDisable());
    }

    public void Init()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        isTrap = false;
        sr.enabled = true;
        team = Team.NONE;
        enterPlayerId = -1;
        id = -1;
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
                SendManager.Instance.Send("ENTER_TRAP", new TrapVO(p.socketId, id));
            }
        }
    }

    public void EnterTrap(int enterPlayerId)
    {
        this.enterPlayerId = enterPlayerId;

        if(co != null)
        {
            StopCoroutine(co);
        }

        co = StartCoroutine(EnterTrapCo());
    }

    IEnumerator EnterTrapCo()
    {
        sr.enabled = true;
        isTrap = true;

        yield return CoroutineHandler.tenSec;

        Init();
    }
}
