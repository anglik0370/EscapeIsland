using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardControllManager : MonoBehaviour
{
    public static KeyBoardControllManager Instance { get; private set; }

    private Player player = null;

    [SerializeField]
    private JoyStick joyStick;

    [SerializeField]
    private Button interactionBtnUI;
    [SerializeField]
    private Button skillBtnUI;

    private InteractionBtn interactionBtn;
    private SkillBtn skillBtn;

    private float h;
    private float v;

    private Vector3 dir;

    private bool isGameStart = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        interactionBtn = interactionBtnUI.transform.GetComponent<InteractionBtn>();
        skillBtn = skillBtnUI.transform.GetComponent<SkillBtn>();
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });

        EventManager.SubGameStart(p => 
        {
            isGameStart = true;
        });

        EventManager.SubGameOver(goc => 
        {
            isGameStart = false;
        });

        EventManager.SubExitRoom(() => 
        {
            isGameStart = false;
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
                player.Move(Vector3.zero);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(interactionBtnUI.interactable && interactionBtn.CanTouch)
            {
                interactionBtnUI.onClick?.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(skillBtnUI.interactable && skillBtn.CanTouch)
            {
                skillBtnUI.onClick?.Invoke();
            }
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (PlayerManager.Instance.Player != null)
            {
                StoragePanel.Instance.Open(PlayerManager.Instance.Player.CurTeam);
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && RefreshUsers.Instance.isTest)
        {
            StorageVO redVO = StorageManager.Instance.StorageDic[Team.RED];

            foreach(var itemAmount in redVO.maxAmountItemList)
            {
                for(int i = 0; i < itemAmount.amount; i++)
                {
                    SendManager.Instance.Send("STORAGE_DROP", new ItemStorageVO(Team.RED, itemAmount.item.itemId));
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.B) && RefreshUsers.Instance.isTest)
        {
            StorageVO blueVO = StorageManager.Instance.StorageDic[Team.BLUE];

            foreach(var itemAmount in blueVO.maxAmountItemList)
            {
                for(int i = 0; i < itemAmount.amount; i++)
                {
                    SendManager.Instance.Send("STORAGE_DROP", new ItemStorageVO(Team.BLUE, itemAmount.item.itemId));
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            if(isGameStart)
            {
                MapPanel.Instance.Open();
            }
        }
    }
}
