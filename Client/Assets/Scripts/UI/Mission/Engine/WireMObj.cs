using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireMObj : MonoBehaviour
{
    private RectTransform rect;
    private Image image;

    public Sprite Sprite => image.sprite;

    [SerializeField]
    private int cuttingOrder;
    public int CuttingOrder => cuttingOrder;

    [SerializeField]
    private bool isCut;
    public bool IsCut => isCut;

    [SerializeField]
    private Vector2 beginPoint;
    [SerializeField]
    private Vector2 endPoint;

    [SerializeField]
    private float yPos = 223;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        beginPoint = new Vector2(rect.anchoredPosition.x, yPos);
        endPoint = new Vector2(rect.anchoredPosition.x, -yPos);
    }

    public void Init()
    {
        cuttingOrder = 0; //�̰� 1���� �����ϴ°� ������ 0�� �ʱ�ȭ��
        isCut = false;
    }

    public void SetCuttingOrder(int cuttingOrder)
    {
        this.cuttingOrder = cuttingOrder;
    }
}
