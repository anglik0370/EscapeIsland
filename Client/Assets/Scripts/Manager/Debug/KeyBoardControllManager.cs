using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardControllManager : MonoBehaviour
{
    public static KeyBoardControllManager Instance { get; private set; }

    private Player player = null;
    private JoyStick joyStick;

    [SerializeField]
    private Button btn;
    private InteractionBtn interactionBtn;

    private float h;
    private float v;

    private Vector3 dir;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        joyStick = FindObjectOfType<JoyStick>();
        interactionBtn = btn.transform.GetComponent<InteractionBtn>();
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });
    }

    private void Update()
    {
        if (player == null || GameManager.Instance.IsPanelOpen) return;

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            dir = new Vector3(h, v, 0).normalized;
            player.Move(dir);
        }
        else
        {
            if(!joyStick.isTouch)
            {
                player.Animator.SetBool("isMoving", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(btn.interactable && interactionBtn.CanTouch)
            {
                btn.onClick?.Invoke();
            }
        }
    }
}
