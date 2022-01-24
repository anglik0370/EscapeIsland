using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBtn : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private Button btn;

    private void Awake() 
    {
        btn = GetComponent<Button>();

        btn.onClick.AddListener(player.PickUpNearlestItem);
    }
}
