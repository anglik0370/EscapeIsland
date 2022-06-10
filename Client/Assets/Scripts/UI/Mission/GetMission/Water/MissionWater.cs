using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionWater : MonoBehaviour, IGetMission, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;

    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [Header("������ ����Ʈ")]
    [SerializeField]
    private ItemGhost itemGhost;

    [Header("������ SO")]
    [SerializeField]
    private ItemSO emptyBottle;
    [SerializeField]
    private ItemSO waterBottle;

    [Header("������")]
    [SerializeField]
    private float maxTime = 2f;
    private float curTime = 0f;

    [SerializeField]
    private bool isPointerInPanel;
    [SerializeField]
    private bool isFilled;

    private BottleGhostMObj bottleGhost;

    [Header("����ġ")]
    [SerializeField]
    private float correctionY = 70;
    [SerializeField]
    private float correctionFrame = 20;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cvs = GetComponent<CanvasGroup>();

        bottleGhost = GetComponentInChildren<BottleGhostMObj>();
    }

    private void Update()
    {
        if(isPointerInPanel)
        {
            if (Input.GetMouseButton(0))
            {
                #region ���� ��ġ ���� �κ�
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
                #endregion

                if (!isFilled)
                {
                    curTime += Time.deltaTime;
                    bottleGhost.SetWaterProgress(curTime / maxTime);

                    if (curTime >= maxTime)
                    {
                        isFilled = true;
                    }
                }
            }
            else
            {
                Close();
            }
        }
    }

    public void Open()
    {
        
    }

    public void Close()
    {
        isPointerInPanel = false;
        isFilled = false;
        curTime = 0f;
        bottleGhost.SetWaterProgress(curTime / maxTime);
        bottleGhost.Disable();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemGhost.GetItem() != emptyBottle) return;

        isPointerInPanel = true;
        itemGhost.Init();
        bottleGhost.Enable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPointerInPanel) return;

        isPointerInPanel = false;

        if(isFilled)
        {
            itemGhost.SetItem(waterBottle);
        }
        else
        {
            itemGhost.SetItem(emptyBottle);
        }

        Close();
    }
}