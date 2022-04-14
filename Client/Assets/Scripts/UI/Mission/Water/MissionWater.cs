using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionWater : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    private Camera mainCam;

    [SerializeField]
    private BottleGhostMObj ghost;

    [SerializeField]
    private bool isPointerInPanel;

    private float correctionY = 70;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        mainCam = Camera.main;
    }

    public void Start()
    {
        Init();

        print(rect.rect.center.x - rect.rect.width / 2);
        print(rect.rect.center.x + rect.rect.width / 2);

        print(rect.rect.center.y - rect.rect.height / 2);
        print(rect.rect.center.y + rect.rect.height / 2);
    }

    private void Update()
    {
        if(isPointerInPanel && Input.GetMouseButton(0))
        {
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            float centerX =  Screen.width / 2;
            float changeX = mouseX - centerX;

            float centerY = Screen.height / 2 + correctionY;
            float changeY = mouseY - centerY;

            float lastX = Mathf.Clamp(changeX,
                                    rect.rect.center.x + ghost.BottleRect.rect.width / 2 - rect.rect.width / 2,
                                    rect.rect.center.x - ghost.BottleRect.rect.width / 2 + rect.rect.width / 2);

            float lastY = Mathf.Clamp(changeY,
                                    rect.rect.center.y + ghost.BottleRect.rect.height / 2 - rect.rect.height / 2,
                                    rect.rect.center.y - ghost.BottleRect.rect.height / 2 + rect.rect.height / 2);

            print(new Vector2(changeX, changeY));

            ghost.SetPosition(new Vector2(lastX + Screen.width / 2, lastY + Screen.height / 2 + correctionY));
        }
    }

    public void Init()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInPanel = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInPanel = false;
    }
}
