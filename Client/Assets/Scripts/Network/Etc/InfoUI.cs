using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    public Transform playerTrm;
    public float followSpeed = 50f;
    public Text txtName;

    private CanvasGroup cvs;

    [SerializeField]
    private Transform mainPlayerTrm;
    [SerializeField]
    private float hideRange; //안보이는 거리

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        hideRange = LightHandler.Instance.lightInnerRadius;

        EventManager.SubTimeChange(isLight =>
        {
            if(isLight)
            {
                hideRange = LightHandler.Instance.lightInnerRadius;
            }
            else
            {
                hideRange = LightHandler.Instance.darkInnerRadius;
            }
        });
    }

    public void SetTarget(Transform playerTrm, Transform mainPlayerTrm, string name)
    {
        this.mainPlayerTrm = mainPlayerTrm;
        this.playerTrm = playerTrm;
        txtName.text = name;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (mainPlayerTrm == null) return;

        if(Vector2.Distance(playerTrm.position, mainPlayerTrm.position) >= hideRange)
        {
            cvs.alpha = 0f;
        }
        else
        {
            cvs.alpha = 1f;
        }
    }

    private void LateUpdate()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(playerTrm.position);
        Vector3 nextPos = Vector3.Lerp(transform.position, pos, Time.deltaTime * followSpeed);
        transform.position = nextPos;
    }
}
