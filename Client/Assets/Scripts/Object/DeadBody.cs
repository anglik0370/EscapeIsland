using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
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

    public void Report()
    {
        MeetManager.Instance.Meet(false);

        foreach(DeadBody deadBody in GameManager.Instance.deadBodyList)
        {
            if(!deadBody.gameObject.activeSelf)
            {
                continue;
            }

            deadBody.gameObject.SetActive(false);
        }

        GameManager.Instance.deadBodyList.Clear();
    }
}
