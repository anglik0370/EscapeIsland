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
    public Text stateText;
    public Image teamImg;
    public RectTransform txtRect;

    private CanvasGroup cvs;

    [SerializeField]
    private Transform mainPlayerTrm;
    [SerializeField]
    private float hideRange; //안보이는 거리

    private Coroutine co;
    private WaitForSeconds stateWs;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        stateWs = new WaitForSeconds(2f);

        SetNameTextColor(Color.gray);
        ClearStateText();

        EventManager.SubExitRoom(() =>
        {
            mainPlayer = player = null;
            playerTrm = mainPlayerTrm = null;
            ClearStateText();
            SetNameTextColor(Color.gray);

            gameObject.SetActive(false);
        });
        EventManager.SubGameOver(goc =>
        {
            if (player != null && !player.master)
            {
                SetNameTextColor(Color.gray);
                ClearStateText();
            }
        });

        EventManager.SubGameInit(() => ClearStateText());
    }

    private void Start()
    {
        hideRange = EyesightManager.Instance.lightInnerRadius;

        EventManager.SubTimeChange(isLight =>
        {
            if (isLight)
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

        Vector2 rectSize = txtRect.sizeDelta;
        rectSize.x = txtName.preferredWidth;
        txtRect.sizeDelta = rectSize;

        Vector2 anchoredPos = txtRect.anchoredPosition;
        anchoredPos.x = 0;
        txtRect.anchoredPosition = anchoredPos;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (mainPlayer == null) return;
        if (mainPlayerTrm == null) return;
        if (mainPlayerTrm == playerTrm) return;

        {
            if (player.Area == mainPlayer.Area)
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
        //Vector3 pos = Camera.main.WorldToScreenPoint(playerTrm.position);
        Vector3 pos = playerTrm.position;
        Vector3 nextPos = Vector3.Lerp(transform.position, pos, Time.deltaTime * followSpeed);
        transform.position = nextPos;
    }

    public void SetNameTextColor(Color color)
    {
        txtName.color = color;
    }

    private void SetStateText(string msg, Color color)
    {
        stateText.text = msg;
        stateText.color = color;
    }

    public void SetState(string msg, Color color, bool isReady = false)
    {
        if (isReady)
        {
            SetStateText(msg, color);
            return;
        }

        if (co != null)
        {
            StopCoroutine(co);
        }

        co = StartCoroutine(SetStateTextCo(msg, color));
    }

    public void ClearStateText()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        SetStateText("", Color.black);
    }

    public void SetTeamImgColor(Color color)
    {
        teamImg.color = color;
    }

    IEnumerator SetStateTextCo(string msg, Color color)
    {
        SetStateText(msg, color);
        yield return stateWs;
        SetStateText("", Color.black);
    }
}
