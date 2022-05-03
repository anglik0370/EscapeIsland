using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IInteractionObject
{
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator anim;
    private Transform playerTrm;
    private Collider2D footCollider;

    public Collider2D FootCollider => footCollider;

    [SerializeField]
    private InteractionSO nothingHandlerSO;

    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => GetInteractionSO();

    public Action LobbyCallback => () => { };
    public Action IngameCallback => () =>
    {
        if(PlayerManager.Instance.AmIKidnapper())
        {
            if(PlayerManager.Instance.AmIDead())
            {
                return;
            }

            PlayerManager.Instance.KillPlayer(this);
        }
    };

    public bool CanInteraction => !isDie && gameObject.activeSelf;

    public string socketName;
    public int socketId;
    public int roomNum;

    private bool isRemote; //true : 다른놈 / false : 조작하는 플레이어
    public bool IsRemote
    {
        get => isRemote;
        set
        {
            isRemote = value;

            if(!isRemote)
            {
                RestartSend();
            }
            else
            {
                StopSend();
            }
        }
    } 

    public bool master;
    public bool isKidnapper; //true : 맢 / false : 시민
    public bool isDie = false;

    public bool isInside = false; //실내인지
    private bool isBone = false;
    public bool isTrap = false;


    public Inventory inventory;
    public Color color;
    public Vector2 targetPos;

    private Vector3 defaultRot;
    private Vector3 flipRot;

    private const float DEFAULT_SCALE_Z = 1;
    private const float FLIP_SCALE_Z = -1;

    public float speed = 5;

    [SerializeField]
    private float range = 5f;

    private WaitForSeconds ws = new WaitForSeconds(1 / 10); //100ms 간격으로 자신의 데이터갱신
    private Coroutine sendData;

    private InfoUI ui = null;
    public InfoUI UI => ui;

    public CharacterSO curSO;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        footCollider = transform.Find("FootCollider").GetComponent<Collider2D>();

        flipRot = new Vector3(0, 180, 0);
        defaultRot = Vector3.zero;
    }

    void OnEnable()
    {
        //임시 더미플레이어 생성
        if (GetChildCount() > 2) return;
        DummyPlayer dummyPlayer = PoolManager.GetItem<DummyPlayer>();

        dummyPlayer.transform.SetParent(transform);

        sr = dummyPlayer.GetComponent<SpriteRenderer>();
        anim = dummyPlayer.GetComponent<Animator>();
        playerTrm = null;
    }

    private void Update()
    {
        if(IsRemote)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector2.Distance(targetPos, transform.position) <= 0.03f)
            {
                anim.SetBool("isMoving", false);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }
        }
    }

    public void InitPlayer(UserVO vo,InfoUI ui, bool isRemote,CharacterSO so)
    {
        this.ui = ui;
        this.curSO = so;
        transform.position = vo.position;
        this.IsRemote = isRemote;
        master = vo.master;

        isTrap = false;
        isBone = false;
        isInside = false;

        socketName = vo.name;
        socketId = vo.socketId;
        roomNum = vo.roomNum;

        if (!isRemote)
        {
            sendData = StartCoroutine(SendData());

            if (curSO != null)
            {
                CharacterProfile pr = CharacterSelectPanel.Instance.GetCharacterProfile(curSO.id);
                pr.BtnEnabled(false);
            }
        }
    }

    public int GetChildCount()
    {
        int count = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }
    
    public int ChangeCharacter(CharacterSO so)
    {
        int beforeSoId = 0;
        //선택되어있던 캐릭터 select button 다시 활성화
        if(curSO != null)
        {
            beforeSoId = curSO.id;
            CharacterProfile pr = CharacterSelectPanel.Instance.GetCharacterProfile(curSO.id);
            pr.BtnEnabled(true);
        }
        curSO = so;

        CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(so.id);
        profile.BtnEnabled(false);
        //플레이어 오브젝트 체인지

        ChangePlayer();

        return beforeSoId;
    }

    public void DeletePlayer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.CompareTag("PlayerPrefab"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ChangePlayer()
    {
        DeletePlayer();

        GameObject player = Instantiate(curSO.playerPrefab, transform);

        sr = player.GetComponent<SpriteRenderer>();
        isBone = sr == null;
        if(isBone)
        {
            playerTrm = player.transform;
        }
        anim = player.GetComponent<Animator>();
    }

    public void RemoveCharacter()
    {
        CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(curSO.id);
        profile.BtnEnabled(true);
        curSO = null;
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Transform GetInteractionTrm()
    {
        return transform;
    }

    public Sprite GetSprite()
    {
        return sr.sprite;
    }

    public bool GetFlipX()
    {
        return sr.flipX;
    }

    public void SetDisable(bool user = false)
    {
        if (!gameObject.activeSelf) return;

        if (user)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(false);
        ui.SetNameTextColor(Color.black);
        ui.gameObject.SetActive(false);
    }

    public void SetEnable()
    {
        anim.SetFloat("isDie", 1f);

        gameObject.SetActive(true);
        ui.gameObject.SetActive(true);
    }

    public void SetDead()
    {
        isDie = true;
        isTrap = false;

        anim.SetFloat("isDie", 1f);
    }

    public void InitPlayer()
    {
        isKidnapper = isDie = false;

        anim.SetFloat("isDie", 0f);
    }

    public void Move(Vector3 dir)
    {
        if(IsRemote || isTrap) return;

        if(dir != Vector3.zero)
        {
            bool isFlip = dir.x > 0;
            if(isBone)
            {
                playerTrm.rotation = Quaternion.Euler(isFlip ? flipRot : defaultRot);
                playerTrm.localScale = new Vector3(playerTrm.localScale.x, playerTrm.localScale.y, isFlip ? FLIP_SCALE_Z : DEFAULT_SCALE_Z);
            }
            else
            {
                if (isFlip)
                {
                    sr.flipX = true;
                }
                else if (dir.x < 0)
                {
                    sr.flipX = false;
                }
            }
            

            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        transform.position += dir * speed * Time.deltaTime;
    }

    IEnumerator SendData()
    {
        while (true)
        {
            yield return ws;

            TransformVO vo = new TransformVO(transform.position, socketId);
            DataVO dataVO = new DataVO("TRANSFORM", JsonUtility.ToJson(vo));

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }
    }

    public void RestartSend()
    {
        StopSend();
        sendData = StartCoroutine(SendData());
    }

    public void StopSend()
    {
        if(sendData != null)
        {
            StopCoroutine(sendData);
        }
    }

    public void SetTransform(Vector2 pos)
    {
        if(IsRemote)
        {
            Vector3 dir = (Vector3)pos - transform.position;

            if(dir != Vector3.zero)
            {
                bool isFlip = dir.x > 0;
                if (isBone)
                {
                    playerTrm.rotation = Quaternion.Euler(isFlip ? flipRot : defaultRot);
                    playerTrm.localScale = new Vector3(playerTrm.localScale.x, playerTrm.localScale.y, isFlip ? FLIP_SCALE_Z : DEFAULT_SCALE_Z);
                }
                else
                {
                    if (isFlip)
                    {
                        sr.flipX = true;
                    }
                    else if (dir.x < 0)
                    {
                        sr.flipX = false;
                    }
                }

                //anim.SetBool("isMoving", true);
            }
            else
            {
                //anim.SetBool("isMoving", false);
            }

            targetPos = pos;
        }
    }

    public bool CheckInRange(IInteractionObject interactionObject)
    {
        if(Vector2.Distance(GetTrm().position, interactionObject.GetInteractionTrm().position) <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private InteractionSO GetInteractionSO()
    {
        if (PlayerManager.Instance.AmIKidnapper())
        {
            if(PlayerManager.Instance.AmIDead())
            {
                return nothingHandlerSO;
            }

             return ingameHandlerSO;
        }

        return nothingHandlerSO;
    }
}
