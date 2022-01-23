using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    RectTransform backgroundRect;
    RectTransform leverRect;

    public Player player;

    public float radius;

    public Vector3 moveDir;
    public Vector3 normal;

    bool isTouch = false;

    private void Start() 
    {
        backgroundRect = transform.Find("background").GetComponent<RectTransform>();
        leverRect = backgroundRect.transform.Find("lever").GetComponent<RectTransform>();

        //반지름을 가져온다
        radius = backgroundRect.rect.width * 0.5f;
    }

    private void Update() 
    {
        if(isTouch && !player.isRemote)
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

        //터치위치 정규화
        normal = leverPos.normalized;

        moveDir = new Vector3(normal.x * player.speed * Time.deltaTime, normal.y * player.speed * Time.deltaTime);
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
        isTouch = false;
    }
}
