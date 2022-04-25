using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WireMObj : MonoBehaviour, IPointerClickHandler
{
    private MissionEngine missionEngine;

    private Image img;
    private RectTransform rect;

    [SerializeField]
    private CanvasGroup cvsOrigin;
    [SerializeField]
    private CanvasGroup cvsCutted;

    [SerializeField]
    private Sprite originSprite;
    public Sprite Sprite => originSprite;

    [SerializeField]
    private int cuttingOrder;
    public int CuttingOrder => cuttingOrder;

    [SerializeField]
    private bool isCut;
    public bool IsCut => isCut;

    [SerializeField]
    private Vector2 beginPoint;
    public Vector2 BeginPoint => beginPoint;
    [SerializeField]
    private Vector2 endPoint;
    public Vector2 EndPoint => endPoint;

    [Header("보정치")]
    [SerializeField]
    private float correctionY = 70;

    private void Awake()
    {
        missionEngine = GetComponentInParent<MissionEngine>();

        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        cvsOrigin = GetComponent<CanvasGroup>();

        originSprite = img.sprite;

        float correctionX = Screen.width / 2;
        float correctionY = Screen.height / 2;

        float endX = correctionX + rect.anchoredPosition.x + rect.rect.width / 2;
        float beginX = correctionX + rect.anchoredPosition.x - rect.rect.width / 2;

        float endY = correctionY + rect.anchoredPosition.y + rect.rect.height / 2;
        float beginY = correctionY + rect.anchoredPosition.y - rect.rect.height / 2;

        beginPoint = new Vector2(beginX, beginY);
        endPoint = new Vector2(endX, endY);
    }

    public void Init()
    {
        UtilClass.SetCanvasGroup(cvsCutted);
        UtilClass.SetCanvasGroup(cvsOrigin, 1, true, true);

        cuttingOrder = 0; //이건 1부터 시작하는게 좋을듯 0은 초기화고
        isCut = false;
    }

    public void SetCuttingOrder(int cuttingOrder)
    {
        this.cuttingOrder = cuttingOrder;
    }

    public void Cut()
    {
        UtilClass.SetCanvasGroup(cvsCutted, 1, false, true);
        UtilClass.SetCanvasGroup(cvsOrigin);

        isCut = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(missionEngine.CurOder == cuttingOrder)
        {
            Cut();
            missionEngine.AddOder();
        }
    }
}
