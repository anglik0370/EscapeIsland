using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineImage : MonoBehaviour
{
    private RectTransform imageRectTransform;
    public float lineWidth = 1.0f;
    public Vector3 pointA;
    public Vector3 pointB;

    void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
