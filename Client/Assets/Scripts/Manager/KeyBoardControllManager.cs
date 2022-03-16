using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardControllManager : MonoBehaviour
{
    private Player player = null;

    [SerializeField]
    private Button interactionBtn;

    private float h;
    private float v;

    private Vector3 dir;

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });
    }

    private void Update()
    {
        if (player == null) return;

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        dir = new Vector3(h, v, 0).normalized;

        player.Move(dir);

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(interactionBtn.interactable)
            {
                interactionBtn.onClick?.Invoke();
            }
        }
    }
}
