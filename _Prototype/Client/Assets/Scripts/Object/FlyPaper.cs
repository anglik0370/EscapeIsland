using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPaper : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    public Team team;
    public int id;
    public int userId;

    private Coroutine co;

    private void Start()
    {
        EventManager.SubGameOver(goc => Init());
        EventManager.SubExitRoom(Init);
    }

    public void Init()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        sr.enabled = true;
        team = Team.NONE;
        id = userId =  -1;
        gameObject.SetActive(false);
    }

    public void SetEnable()
    {
        gameObject.SetActive(true);
        if (co != null)
        {
            StopCoroutine(co);
        }
        co = StartCoroutine(SetDisable());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player p = col.transform.parent.GetComponentInParent<Player>();

            if (p != null && !p.IsRemote && !p.CurTeam.Equals(team))
            {
                SendManager.Instance.EnterFlyPaper(p.socketId, id, userId);
            }
        }
    }

    IEnumerator SetDisable()
    {
        sr.enabled = true;
        if (NetworkManager.instance.User.CurTeam.Equals(team)) yield break;

        yield return CoroutineHandler.oneSec;
        sr.enabled = false;
    }
}
