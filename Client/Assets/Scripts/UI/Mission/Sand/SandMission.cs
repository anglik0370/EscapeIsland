using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandMission : MonoBehaviour, IMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    private SandCircleMObj circle;
    private SandBucketMObj bucket;

    [SerializeField]
    private CanvasGroup cvsSandSlot;
    [SerializeField]
    private MissionDropItemSlot slot;

    [Header("양동이에 모은 모래")]
    [SerializeField]
    private float maxSand = 1f;
    [SerializeField]
    private float curSand = 0f;

    [Header("판정당 얼마나 줄지")]
    [SerializeField]
    private float halfSand = 0.5f;
    [SerializeField]
    private float quaterSand = 0.25f;

    [Header("풀링 범위")]
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxY;
    [SerializeField]
    private float minY;

    private Coroutine co;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        circle = GetComponentInChildren<SandCircleMObj>();
        bucket = GetComponentInChildren<SandBucketMObj>();
    }

    private void Start()
    {
        circle.OccurRoutineComplete(isPerfect =>
        {
            if(isPerfect)
            {
                curSand += halfSand;
            }
            else
            {
                curSand += quaterSand;
            }

            curSand = Mathf.Clamp(curSand, 0f, maxSand);

            bucket.UpdateFillAmount(curSand, maxSand);

            if (curSand < maxSand)
            {
                StartEnableCircleRoutine();
            }
            else
            {
                UtilClass.SetCanvasGroup(cvsSandSlot, 1, true, true, false);
            }
        }, () =>
        {
            if(curSand < maxSand)
            {
                StartEnableCircleRoutine();
            }
            else
            {
                UtilClass.SetCanvasGroup(cvsSandSlot, 1, true, true, false);
            }
        });
    }

    public void Init()
    {
        curSand = 0f;

        UtilClass.SetCanvasGroup(cvsSandSlot);
        slot.Init();
        slot.SetRaycastTarget(true);

        bucket.UpdateFillAmount(curSand, maxSand);

        StartEnableCircleRoutine();
    }

    private void StartEnableCircleRoutine()
    {
        if(co != null)
        {
            StopCoroutine(co);
        }

        StartCoroutine(EnableCircleRoutine());
    }

    private IEnumerator EnableCircleRoutine()
    {
        Vector2 ranPos;

        ranPos.x = UnityEngine.Random.Range(minX, maxX);
        ranPos.y = UnityEngine.Random.Range(minY, maxY);

        yield return new WaitForSeconds(0.5f); //최초로는 1초 기다렸다가

        circle.Init();
        circle.SetPosition(ranPos);
        circle.StartShrink();
    }
}
