using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireMObj : MonoBehaviour
{
    private RectTransform rect;
    private Image image;

    public Sprite Sprite => originSprite;

    [SerializeField]
    private int cuttingOrder;
    public int CuttingOrder => cuttingOrder;

    [SerializeField]
    private bool isCut;
    public bool IsCut => isCut;

    [SerializeField]
    private Sprite originSprite;
    [SerializeField]
    private Sprite cutSprite;

    [SerializeField]
    private Vector2 beginPoint;
    public Vector2 BeginPoint => beginPoint;
    [SerializeField]
    private Vector2 endPoint;
    public Vector2 EndPoint => endPoint;

    [Header("����ġ")]
    [SerializeField]
    private float correctionY = 70;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();

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
        cuttingOrder = 0; //�̰� 1���� �����ϴ°� ������ 0�� �ʱ�ȭ��
        isCut = false;

        image.sprite = originSprite;
    }

    public void SetCuttingOrder(int cuttingOrder)
    {
        this.cuttingOrder = cuttingOrder;
    }

    public void Cut()
    {
        image.sprite = cutSprite;
        isCut = true;
    }
}
