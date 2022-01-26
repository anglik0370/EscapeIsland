using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaEntrance : MonoBehaviour
{
    private AreaCover cover;

    private bool isEntering = false;

    private void Awake() 
    {
        cover = GetComponentInParent<AreaCover>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(isEntering) return;

        if(other.gameObject.CompareTag("Player"))
        {
            if(!other.GetComponent<Player>().isRemote)
            {
                cover.Enter();
                isEntering = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!other.GetComponent<Player>().isRemote)
            {
                isEntering = false;
            }
        }
    }
}
