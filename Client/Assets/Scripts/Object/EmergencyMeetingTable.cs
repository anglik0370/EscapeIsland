using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyMeetingTable : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Sprite GetSprite()
    {
        return sr.sprite;
    }

    public void Meeting()
    {
        //긴급회의 할떄 어케할지 여따가 하면 댐
    }
}
