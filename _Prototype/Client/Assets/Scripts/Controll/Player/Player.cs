using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IInteractionObject
{
    private const string ANIMB_MOVE = "isMoving";
    private const string ANIMB_DIE = "isDie";
    private const string ANIMT_ATTACK = "attack";

    private BuffHandler buffHandler;
    public BuffHandler BuffHandler => buffHandler;

    private Animator anim;
    private Transform playerTrm;
    private Collider2D footCollider;
    private Collider2D bodyCollider;

    public Collider2D FootCollider => footCollider;
    public Collider2D BodyCollider => bodyCollider;

    public Animator Animator => anim;

    private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

    [SerializeField]
    private InteractionSO nothingHandlerSO;

    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => ingameHandlerSO;

    public Action LobbyCallback => () => { };
    public Action IngameCallback => () => { };

    public bool CanInteraction => gameObject.activeSelf;

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
    public bool isReady = false;

    private Team curTeam = Team.NONE;
    public Team CurTeam
    {
        get => curTeam;
        set => curTeam = value;
    }

    public bool canMove = false;
    private bool isRestrict = false;

    public bool IsSturned => !isRestrict && !canMove;
    public bool IsRestrict => isRestrict && !canMove;

    private bool canGathering; //채집 가능한지
    public bool CanGathering => canGathering;

    public bool isFlip = false; //뒤집혔는지
    public bool isNotLerp = false;

    public Inventory inventory;
    public Color color;
    public Vector2 targetPos;

    private Vector3 defaultRot;
    private Vector3 flipRot;

    private Vector3 defaultPos;
    private Vector3 flipPos;

    private const float DEFAULT_SCALE_Z = 1;
    private const float FLIP_SCALE_Z = -1;

    [SerializeField]
    private float speed = 6;
    public float Speed => speed;

    [SerializeField]
    private float coolTimeMagnification = 1f;
    public float CoolTimeMagnification => coolTimeMagnification;

    private float originSpeed = 0f;

    private WaitForSeconds ws = new WaitForSeconds(1 / 10); //100ms 간격으로 자신의 데이터갱신
    private Coroutine sendData;
    private Coroutine co;

    private InfoUI ui = null;
    public InfoUI UI => ui;

    private TeamInfoUI teamUI = null;
    public TeamInfoUI TeamUI => teamUI;

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

        interactionCol = GetComponentInChildren<Collider2D>();

        audioSource = GetComponent<AudioSource>();
        buffHandler = GetComponent<BuffHandler>();

        originSpeed = speed;
    }
    private void Update()
    {
        if(IsRemote && !isNotLerp)
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

    public void ChangeUI(UserVO vo,bool lobbyUIRefresh = false)
    {
        bool isBlueTeam = vo.curTeam.Equals(Team.BLUE);
        isReady = vo.ready;
        ui.SetTeamImgColor(isBlueTeam ? Color.blue : Color.red);
        teamUI.SetParent(TeamPanel.Instance.GetParent(isBlueTeam));
        curTeam = isBlueTeam ? Team.BLUE : Team.RED;

        if(!master)
        {
            teamUI.SetReadyText(vo.ready, UtilClass.READY_TEXT);
            ui.SetNameTextColor(vo.ready ? Color.black : Color.grey);
        }

        if (lobbyUIRefresh) return;

        ChangeCharacter(CharacterSelectPanel.Instance.GetDefaultProfile().GetSO());
    }

    public void InitPlayer(UserVO vo,InfoUI ui,TeamInfoUI teamUI, bool isRemote,CharacterSO so)
    {
        this.ui = ui;
        this.teamUI = teamUI;
        this.curSO = so;
        transform.position = vo.position;
        this.IsRemote = isRemote;
        master = vo.master;

        this.curTeam = vo.curTeam;

        canMove = true;

        socketName = vo.name;
        socketId = vo.socketId;
        roomNum = vo.roomNum;

        ChangeUI(vo,true);

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

        footCollider = dummyPlayer.transform.Find("FootCollider").GetComponent<Collider2D>();
        bodyCollider = dummyPlayer.transform.Find("BodyCollider").GetComponent<Collider2D>();
        playerTrm = dummyPlayer.transform;

        defaultPos = dummyPlayer.transform.localPosition;
        flipPos = new Vector3(-curSO.adjsutPos.x, curSO.adjsutPos.y, curSO.adjsutPos.z);

        anim.ResetTrigger(ANIMT_ATTACK);

        curSO.skill.InitTimer(); //스킬 쿨 초기화

        dummyPlayer.SetActive(true);
    }

    public void PlayVoice(AudioClip clip)
    {
        audioSource.clip = clip;

        audioSource.Play();
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
        if (so == null) return 0;

        int beforeSoId = 0;
        bool isSameTeam = this.curTeam.Equals(NetworkManager.instance.User.CurTeam);

        //선택되어있던 캐릭터 select button 다시 활성화
        if (curSO != null)
        {
            beforeSoId = curSO.id;
            CharacterProfile pr = CharacterSelectPanel.Instance.GetCharacterProfile(curSO.id);

            if(isSameTeam)
                pr.BtnEnabled(true);
        }
        curSO = so;

        CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(so.id);

        if(isSameTeam)
            profile.BtnEnabled(false);
        //플레이어 오브젝트 체인지

        teamUI.RefreshProfile();

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

        gameObject.SetActive(true);
        ui.gameObject.SetActive(true);
    }

    public void SetAttack()
    {
        anim.SetTrigger(ANIMT_ATTACK);
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

    public void SetPosition(Vector2 pos)
    {
        isNotLerp = true;
        transform.position = pos;
        targetPos = pos;
        isNotLerp = false;
    }

    public void SetAreaState(AreaState areaState)
    {
        this.areaState = areaState;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetMagnification(float magnification)
    {
        this.coolTimeMagnification = magnification;
    }

    public void SetOriginSpeed()
    {
        this.speed = originSpeed;
    }
    
    public void SetRestrict(bool on)
    {
        if(co != null && on)
        {
            StopCoroutine(co);
        }

        isRestrict = on;
    }

    public void StartOffRestrict(float time)
    {
        co = StartCoroutine(RestrictOff(time));
    }

    IEnumerator RestrictOff(float time)
    {
        yield return new WaitForSeconds(time);

        SetRestrict(false);
    }

    public void SetGathering(bool canGathering)
    {
        this.canGathering = canGathering;
    }
}
