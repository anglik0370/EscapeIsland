using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private CanvasGroup cvs;

    private void Awake() 
    {
        cvs = GetComponent<CanvasGroup>();

        Close();
    }

    public void Open()
    {
        cvs.alpha = 1f;
        cvs.blocksRaycasts = true;
        cvs.interactable = true;
    }

    public void Close()
    {
        cvs.alpha = 0f;
        cvs.blocksRaycasts = false;
        cvs.interactable = false;
    }
}
