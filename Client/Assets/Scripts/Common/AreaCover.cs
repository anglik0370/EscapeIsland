using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AreaCover : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    private Color coverColor;
    private Color disCoverColor;

    private const float DURATION = 1f;
    private bool isCovered = true; //덮여있는지

    public Player player;

    private Sequence seq;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        seq = DOTween.Sequence();

        coverColor = sr.color;
        disCoverColor = new Color(coverColor.r, coverColor.g, coverColor.b, 0);

        isCovered = true;
    }

    private void Start()
    {
        EventManager.SubEnterRoom(SetPlayer);
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    private void Update()
    {
        if (player == null) return;

        if(isCovered)
        {
            if (col.bounds.Contains(player.GetTrm().position))
            {
                //덮여있는데 안에있는 상태
                seq.Kill();
                seq = DOTween.Sequence();
                seq.Append(sr.DOColor(disCoverColor, DURATION));

                isCovered = false;

                print("들어옴");
                player.isInside = true;
                SendManager.Instance.SendAreaState(player.AreaState);
            }
        }
        else
        {
            if (!col.bounds.Contains(player.GetTrm().position))
            {
                //안덮여있는데 안에없는 상태
                seq.Kill();
                seq = DOTween.Sequence();
                seq.Append(sr.DOColor(coverColor, DURATION));

                isCovered = true;

                print("나감");
                player.isInside = false;
                SendManager.Instance.SendAreaState(player.AreaState);
            }
        }
    }
}
