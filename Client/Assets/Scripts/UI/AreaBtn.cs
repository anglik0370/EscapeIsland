using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaBtn : MonoBehaviour
{
    private Button btn;
    public AreaSO areaSO;

    private void Awake() 
    {
        btn = GetComponent<Button>();
    }

    private void Start() 
    {
        btn.onClick.AddListener(() => AreaExplanationPanel.Instance.Open(areaSO));
    }
}
