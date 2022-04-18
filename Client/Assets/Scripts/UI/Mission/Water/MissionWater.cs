using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionWater : MonoBehaviour, IMission, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;

    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [Header("아이템 고스트")]
    [SerializeField]
    private ItemGhost itemGhost;

    [Header("아이템 SO")]
    [SerializeField]
    private ItemSO emptyBottle;
    [SerializeField]
    private ItemSO waterBottle;

    //[Header("미션관련 오브젝트")]
    private BottleGhostMObj bottleGhost;
    private SeaMObj sea;

    [Header("보정치")]
    [SerializeField]
    private float correctionY = 70;
    [SerializeField]
    private float correctionFrame = 20;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    [SerializeField]
    private bool isPointerInPanel;
    public bool IsPointerInPanel => isPointerInPanel;

    [SerializeField]
    private bool isDragging;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cvs = GetComponent<CanvasGroup>();

        sea = GetComponentInChildren<SeaMObj>();
        bottleGhost = GetComponentInChildren<BottleGhostMObj>();
    }

    public void Start()
    {
        Init();
    }

    private void Update()
    {
        if(isPointerInPanel && isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                float mouseX = Input.mousePosition.x;
                float mouseY = Input.mousePosition.y;

                float centerX = Screen.width / 2;
                float changeX = mouseX - centerX;

                float centerY = Screen.height / 2 + correctionY;
                float changeY = mouseY - centerY;

                float lastX = Mathf.Clamp(changeX,
                                        rect.rect.center.x - correctionFrame + bottleGhost.BottleRect.rect.width / 2 - rect.rect.width / 2,
                                        rect.rect.center.x + correctionFrame - bottleGhost.BottleRect.rect.width / 2 + rect.rect.width / 2);

                float lastY = Mathf.Clamp(changeY,
                                        rect.rect.center.y - correctionFrame + bottleGhost.BottleRect.rect.height / 2 - rect.rect.height / 2,
                                        rect.rect.center.y + correctionFrame - bottleGhost.BottleRect.rect.height / 2 + rect.rect.height / 2);

                bottleGhost.SetPosition(new Vector2(lastX + Screen.width / 2, lastY + Screen.height / 2 + correctionY));
            }
            else
            {
                Init();
            }
        }
    }

    public void Init()
    {
        isPointerInPanel = false;
        isDragging = false;
        bottleGhost.Init();
        sea.Init();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInPanel = true;

        if (Input.GetMouseButton(0) && itemGhost.GetItem() == emptyBottle)
        {
            isDragging = true;

            bottleGhost.Enable();
            bottleGhost.SetPosition(eventData.position);

            itemGhost.Init();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPointerInPanel && isDragging)
        {
            if (bottleGhost.isFilled())
            {
                print("꽉찼음");
                itemGhost.SetItem(waterBottle);
            }
            else
            {
                print("꽉 안찼음");
                itemGhost.SetItem(emptyBottle);
            }

            Init();
        }
    }
}
