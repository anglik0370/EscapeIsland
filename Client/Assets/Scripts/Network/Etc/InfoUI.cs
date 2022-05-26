using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Player mainPlayer = null;
    public Player MainPlayer => mainPlayer;

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

        EventManager.SubExitRoom(() => SetNameTextColor(Color.black));
        EventManager.SubGameOver(goc => SetNameTextColor(Color.black));
    }

    private void Start()
    {
        hideRange = EyesightManager.Instance.lightInnerRadius;

        EventManager.SubTimeChange(isLight =>
        {
            if(isLight)
            {
                hideRange = EyesightManager.Instance.lightInnerRadius;
            }
            else
            {
                hideRange = EyesightManager.Instance.darkInnerRadius;
            }
        });
    }

    public void SetTarget(Transform playerTrm, Player mainPlayer, string name)
    {
        this.mainPlayerTrm = mainPlayer.transform;
        this.mainPlayer = mainPlayer;
        this.playerTrm = playerTrm;

        player = playerTrm.GetComponent<Player>();

        txtName.text = name;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (mainPlayer == null) return;
        if (mainPlayerTrm == null) return;
        if (mainPlayerTrm == playerTrm) return;
        
        if(mainPlayer.isDie)
        {
            cvs.alpha = 1f;
        }
        else
        {
            if (player.AreaState == mainPlayer.AreaState)
            {
                if (Vector2.Distance(playerTrm.position, mainPlayerTrm.position) >= hideRange)
                {
                    cvs.alpha = 0f;
                }
                else
                {
                    cvs.alpha = 1f;
                }
            }
            else
            {
                cvs.alpha = 0f;
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(playerTrm.position);
        Vector3 nextPos = Vector3.Lerp(transform.position, pos, Time.deltaTime * followSpeed);
        transform.position = nextPos;
    }

    public void SetNameTextColor(Color color)
    {
        txtName.color = color;
    }
}
