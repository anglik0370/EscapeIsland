
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator anim;

    public string socketName;
    public int socketId;

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

    private InfoUI ui = null;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isRemote)
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

    public void InitPlayer(UserVO vo,InfoUI ui, bool isRemote)
    {
        this.ui = ui;
        transform.position = vo.position;
        this.isRemote = isRemote;
        master = vo.master;
        socketName = vo.name;
        socketId = vo.socketId;

        if (!isRemote)
        {
            sendData = StartCoroutine(SendData());
        }
    }

    public void SetDisable()
    {
        if (!gameObject.activeSelf) return;

        gameObject.SetActive(false);
        ui.gameObject.SetActive(false);
    }
    public void SetEnable()
    {
        gameObject.SetActive(true);
        ui.gameObject.SetActive(true);
    }

    public void SetDead()
    {
        isDie = true;
    }

    public void Move(Vector3 dir)
    {
        if(isRemote) return;

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
            Vector3 dir = (Vector3)pos - transform.position;

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

                

                //anim.SetBool("isMoving", true);
            }
            else
            {
                //anim.SetBool("isMoving", false);
            }

            targetPos = pos;
        }
    }
}
