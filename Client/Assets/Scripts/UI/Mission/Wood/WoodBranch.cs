using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WoodBranch : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;

    [SerializeField]
    private Vector2 checkLine;

    private void Awake()
    {
        checkLine = endPoint.position - startPoint.position;
    }

    public void Move(Transform targetTrm)
    {
        transform.DOMove(targetTrm.position, 0.5f);
    }
}
