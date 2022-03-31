using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    RectTransform parentRect;
    RectTransform backgroundRect;
    RectTransform leverRect;

    private readonly Vector3 MIN_SCALE = new Vector3(0.5f, 0.5f, 1);

    public Player player;

    public float radius;

    public Vector3 moveDir;
    public Vector3 normal;

    bool isTouch = false;

    private void Start() 
    {
        parentRect = GetComponent<RectTransform>();
        backgroundRect = transform.Find("background").GetComponent<RectTransform>();
        leverRect = backgroundRect.transform.Find("lever").GetComponent<RectTransform>();

        //반지름을 가져온다
        radius = backgroundRect.rect.width * 0.5f;

        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });
    }

    private void Update() 
    {
        if(isTouch && !GameManager.Instance.IsPanelOpen)
        {
            player.Move(moveDir);
        }
    }

    void OnTouch(Vector2 touch)
    {
        Vector2 leverPos = new Vector2(touch.x - backgroundRect.position.x, touch.y - backgroundRect.position.y);

        //레버의 위치가 반지름을 넘어가지 않게
        leverPos = Vector2.ClampMagnitude(leverPos, radius);
        leverRect.localPosition = leverPos;

        moveDir = new Vector3(leverPos.x, leverPos.y).normalized;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        isTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        leverRect.localPosition = Vector2.zero;
        moveDir = Vector2.zero;
        player.Move(moveDir);

        isTouch = false;
    }

    public void SetEnable(bool on)
    {
        this.enabled = on;

        if(!on)
        {
            leverRect.localPosition = Vector2.zero;
            moveDir = Vector2.zero;
        }
    }

    public void SetLocalScale(float add)
    {
        //0.5에서 1.5의 크기가 되도록 만들면 될듯?

        parentRect.localScale = new Vector3(MIN_SCALE.x + add, MIN_SCALE.y + add, MIN_SCALE.z);
    }
}
