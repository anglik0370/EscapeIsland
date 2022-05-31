using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteTimeBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform parentRect;

    [SerializeField]
    private RectTransform remainTimeBar;

    [SerializeField]
    private RectTransform voteTimeRect;
    [SerializeField]
    private RectTransform discussTimeRect;

    [SerializeField]
    private Text voteTimeTxt;
    [SerializeField]
    private Text discussionTimeTxt;

    public void Init(float discussTime, float voteTime)
    {
        discussionTimeTxt.text = $"{discussTime}√ ";
        voteTimeTxt.text = $"{voteTime}√ ";

        float totlaTime = discussTime + voteTime;

        float discussLength = (discussTime / totlaTime) * parentRect.rect.width;
        float voteLength = (voteTime / totlaTime) * parentRect.rect.width;

        //discussTimeRect.offsetMin = new Vector2()
    }
}
