
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator anim;

    public bool isRemote; //true : 다른놈 / false : 조작하는 플레이어
    public bool master;
    public bool isImposter; //true : 맢 / false : 시민
    public bool isDie = false;

    public Inventory inventory;
    public Color color;
    public Vector2 targetPos;

    public float speed = 5;

    public float range = 5f;

    private WaitForSeconds ws = new WaitForSeconds(1 / 5); //200ms 간격으로 자신의 데이터갱신
    private Coroutine sendData;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if(!isRemote)
        {

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    public void InitPlayer(UserVO vo, bool isRemote)
    {
        transform.position = vo.position;
        this.isRemote = isRemote;
        master = vo.master;

        

        if(!isRemote)
        {
            sendData = StartCoroutine(SendData());
        }
    }

    public void SetDisable()
    {
        if (!gameObject.activeSelf) return;

        gameObject.SetActive(false);
    }

    public void SetDead()
    {
        isDie = true;
        NetworkManager.instance.Die();
    }

    public void Move(Vector3 dir)
    {
        if(dir != Vector3.zero)
        {
            if(dir.x > 0)
            {
                sr.flipX = true;
            }
            else if (dir.x < 0)
            {
                sr.flipX = false;
            }

            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        transform.position += dir * speed * Time.deltaTime;
    }

    private void SetAnimation(Vector2 dir)
    {
        
    }

    IEnumerator SendData()
    {
        int socketId = NetworkManager.instance.socketId;
        string socketName = NetworkManager.instance.socketName;
        int roomNum = NetworkManager.instance.roomNum;
        while (true)
        {
            yield return ws;

            TransformVO vo = new TransformVO(transform.position, socketId);
            string payload = JsonUtility.ToJson(vo);
            DataVO dataVO = new DataVO("TRANSFORM", payload);

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }
    }

    public void StopCo()
    {
        if(sendData != null)
        {
            StopCoroutine(sendData);
        }
    }

    public void SetTransform(Vector2 pos)
    {
        if(isRemote)
        {
            Vector2 dir = (pos - (Vector2)transform.position);

            if(dir != Vector2.zero)
            {
                if (dir.x > 0)
                {
                    sr.flipX = true;
                }
                else if (dir.x < 0)
                {
                    sr.flipX = false;
                }
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            targetPos = pos;
        }
    }
}
