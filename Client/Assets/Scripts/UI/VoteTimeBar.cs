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
    private RectTransform baseLineRect;

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

        print($"{discussLength}, {voteLength}");

        discussTimeRect.offsetMin = new Vector2(voteLength, discussTimeRect.offsetMin.y);
        voteTimeRect.offsetMax = new Vector2(-discussLength, voteTimeRect.offsetMax.y);

        baseLineRect.anchoredPosition = new Vector3(voteLength, 0, 0);
    }

    public void UpdateTimerUI(float totalTime, float currentTime)
    {
        remainTimeBar.localScale = new Vector3(currentTime / totalTime, 1, 1);
    }
}
