using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AreaCover : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField]
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
            if (Physics2D.IsTouching(col, player.FootCollider))
            {
                //덮여있는데 안에있는 상태
                seq.Kill();
                seq = DOTween.Sequence();
                seq.Append(sr.DOColor(disCoverColor, DURATION));

                isCovered = false;
            }
        }
        else
        {
            if (!Physics2D.IsTouching(col, player.FootCollider))
            {
                //안덮여있는데 안에없는 상태
                seq.Kill();
                seq = DOTween.Sequence();
                seq.Append(sr.DOColor(coverColor, DURATION));

                isCovered = true;
            }
        }
    }
}
