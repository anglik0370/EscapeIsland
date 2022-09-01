using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMonkey : MonoBehaviour
{
    private const string ANIM_MOVE_NAME = "isMoving";

    private const float Y_POS = -3.16f;

    private const float ORIGIN_X_POS = 0;
    private const float FLIP_X_POS = 3.5f;

    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void FlipSprite(bool isFlip)
    {
        if(isFlip)
        {
            transform.localPosition = new Vector3(FLIP_X_POS, Y_POS, 0);
        }
        else
        {
            transform.localPosition = new Vector3(ORIGIN_X_POS, Y_POS, 0);
        }

        sr.flipX = true;
    }

    public void SetAnimation(bool isMove)
    {
        anim.SetBool(ANIM_MOVE_NAME, isMove);
    }
}
