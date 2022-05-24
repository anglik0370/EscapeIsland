using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IInteractionObject
{
    private const string ANIMB_MOVE = "isMoving";
    private const string ANIMB_DIE = "isDie";
    private const string ANIMT_ATTACK = "attack";

    private Animator anim;
    private Transform playerTrm;
    private Collider2D footCollider;
    private Collider2D bodyCollider;

    public Collider2D FootCollider => footCollider;
    public Collider2D BodyCollider => bodyCollider;

    public Animator Animator => anim;

    private AudioSource voiceSource;

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

    [SerializeField]
    private Collider2D interactionCol;
    public Collider2D InteractionCol => interactionCol;

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

    public bool canMove = false;
    public bool isFlip = false; //뒤집혔는지

    public Inventory inventory;
    public Color color;
    public Vector2 targetPos;

    private Vector3 defaultRot;
    private Vector3 flipRot;

    private Vector3 defaultPos;
    private Vector3 flipPos;

    private const float DEFAULT_SCALE_Z = 1;
    private const float FLIP_SCALE_Z = -1;

    private int defaultBodyLayer = -1; 
    private int defaultFootLayer = -1;
    private int deadLayer = -1;

    //이건 죽었을 때 고스트 이미지 보이게 하는 용도임
    private List<SpriteRenderer> srList;
    private SpriteRenderer ghostSr;

    public float speed = 5;

    private WaitForSeconds ws = new WaitForSeconds(1 / 10); //100ms 간격으로 자신의 데이터갱신
    private Coroutine sendData;

    private InfoUI ui = null;
    public InfoUI UI => ui;

    [SerializeField]
    private AreaState areaState;
    public AreaState AreaState
    {
        get => areaState;
        set => areaState = value;
    }

    public CharacterSO curSO;

    private void Awake()
    {
        flipRot = new Vector3(0, 180, 0);
        defaultRot = Vector3.zero;

        defaultBodyLayer = LayerMask.NameToLayer("PLAYER");
        defaultFootLayer = LayerMask.NameToLayer("PLAYERFOOT");
        deadLayer = LayerMask.NameToLayer("PLAYERGHOST");

        interactionCol = GetComponentInChildren<Collider2D>();

        voiceSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EventManager.SubGameOver(goc =>
        {
            for (int i = 0; i < srList.Count; i++)
            {
                srList[i].color = UtilClass.opacityColor;
            }

            ghostSr.color = UtilClass.limpidityColor;
        });
    }

    private void Update()
    {
        if(IsRemote)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector2.Distance(targetPos, transform.position) <= 0.03f)
            {
                anim.SetBool(ANIMB_MOVE, false);
            }
            else
            {
                anim.SetBool(ANIMB_MOVE, true);
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

        canMove = true;

        socketName = vo.name;
        socketId = vo.socketId;
        roomNum = vo.roomNum;

        if (!isRemote)
        {
            if (curSO != null)
            {
                CharacterProfile pr = CharacterSelectPanel.Instance.GetCharacterProfile(curSO.id);
                pr.BtnEnabled(false);
            }
        }

        CreateCharacter();
    }

    public void CreateCharacter()
    {
        GameObject dummyPlayer = CharacterSelectPanel.Instance.GetCharacterObj(curSO.id);

        dummyPlayer.transform.SetParent(transform);
        dummyPlayer.transform.localPosition = curSO.adjsutPos;

        anim = dummyPlayer.GetComponent<CharComponentHolder>().anim;

        srList = dummyPlayer.GetComponent<CharComponentHolder>().srList;
        ghostSr = dummyPlayer.transform.Find("Ghost").GetComponent<SpriteRenderer>();

        for (int i = 0; i < srList.Count; i++)
        {
            srList[i].color = UtilClass.opacityColor;
        }

        ghostSr.color = UtilClass.limpidityColor;

        footCollider = dummyPlayer.transform.Find("FootCollider").GetComponent<Collider2D>();
        bodyCollider = dummyPlayer.transform.Find("BodyCollider").GetComponent<Collider2D>();
        playerTrm = dummyPlayer.transform;

        defaultPos = dummyPlayer.transform.localPosition;
        flipPos = new Vector3(-curSO.adjsutPos.x, curSO.adjsutPos.y, curSO.adjsutPos.z);

        anim.ResetTrigger(ANIMT_ATTACK);

        dummyPlayer.SetActive(true);
    }

    public void PlayVoice(AudioClip clip)
    {
        voiceSource.clip = clip;

        voiceSource.Play();
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

        CreateCharacter();
    }

    public void RemoveCharacter()
    {
        CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(curSO.id);

        if (profile == null) return;

        profile.BtnEnabled(true);
        curSO = null;
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Sprite GetSprite()
    {
        return null;
    }

    public CharComponentHolder GetCCH()
    {
        return GetComponentInChildren<CharComponentHolder>();
    }

    public bool GetFlipX()
    {
        return isFlip;
    }

    public void SetDisable(bool user = false)
    {
        if (!gameObject.activeSelf) return;

        ChangeLayer(false);

        if (user)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        ui.SetNameTextColor(Color.black);
        ui.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetEnable()
    {
        anim.SetFloat(ANIMB_DIE, 0f);
        ChangeLayer(true);

        gameObject.SetActive(true);
        ui.gameObject.SetActive(true);
    }

    public void SetDead()
    {
        isDie = true;
        canMove = true;

        for (int i = 0; i < srList.Count; i++)
        {
            srList[i].color = UtilClass.limpidityColor;
        }

        ghostSr.color = UtilClass.opacityColor;

        anim.SetFloat(ANIMB_DIE, 1f);

        ChangeLayer(true);
    }

    public void SetAttack()
    {
        anim.SetTrigger(ANIMT_ATTACK);
    }

    private void ChangeLayer(bool isDie)
    {
        bodyCollider.gameObject.layer = isDie ? deadLayer : defaultBodyLayer;
        footCollider.gameObject.layer = isDie ? deadLayer : defaultFootLayer;
    }

    public void InitPlayer()
    {
        isKidnapper = isDie = false;

        anim.SetFloat(ANIMB_DIE, 0f);
        ChangeLayer(false);
    }

    public void Move(Vector3 dir)
    {
        if(IsRemote || !canMove) return;

        if(dir != Vector3.zero)
        {
            if (dir.x != 0)
            {
                isFlip = dir.x > 0;
            }

            playerTrm.rotation = Quaternion.Euler(isFlip ? flipRot : defaultRot);
            playerTrm.localPosition = isFlip ? flipPos : defaultPos;
            playerTrm.localScale = new Vector3(playerTrm.localScale.x, playerTrm.localScale.y, isFlip ? FLIP_SCALE_Z : DEFAULT_SCALE_Z);

            anim.SetBool(ANIMB_MOVE, true);
        }
        else
        {
            anim.SetBool(ANIMB_MOVE, false);
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
                if(dir.x != 0)
                {
                    isFlip = dir.x > 0;
                }

                playerTrm.rotation = Quaternion.Euler(isFlip ? flipRot : defaultRot);
                playerTrm.localPosition = isFlip ? flipPos : defaultPos;
                playerTrm.localScale = new Vector3(playerTrm.localScale.x, playerTrm.localScale.y, isFlip ? FLIP_SCALE_Z : DEFAULT_SCALE_Z);

                //anim.SetBool("isMoving", true);
            }
            else
            {
                //anim.SetBool("isMoving", false);
            }

            targetPos = pos;
        }
    }

    public void SetAreaState(AreaState areaState)
    {
        this.areaState = areaState;
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
