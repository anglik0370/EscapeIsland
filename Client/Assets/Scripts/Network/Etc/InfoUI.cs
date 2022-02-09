using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    public Transform playerTrm;
    public float followSpeed = 50f;
    public Text txtName;

    public void SetTarget(Transform playerTrm, string name)
    {
        this.playerTrm = playerTrm;
        txtName.text = name;
        gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(playerTrm.position);
        Vector3 nextPos = Vector3.Lerp(transform.position, pos, Time.deltaTime * followSpeed);
        transform.position = nextPos;
    }
}
