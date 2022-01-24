
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;

    public bool isRemote; //true : 다른놈 / false : 조작하는 플레이어
    public bool master;

    public Inventory inventory;
    public Color color;
    public Vector2 targetPos;

    [SerializeField]
    public int speed = 5;

    private WaitForSeconds ws = new WaitForSeconds(1 / 5); //200ms 간격으로 자신의 데이터갱신

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        #if UNITY_EDITOR

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, v, 0).normalized * speed * Time.deltaTime;

        Move(dir);

        #endif

        if(!isRemote)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PickUpNearlestItem();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                PutItemInStorage();
            }
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
            StartCoroutine(SendData());
        }
    }

    public void Move(Vector3 dir)
    {
        transform.position += dir;
    }

    public void PickUpNearlestItem()
    {
        //디버그용 함수로 가장 가까이에 있는 아이템을 인벤토리에 넣습니다

        ItemSpawner spawner = GameObject.FindObjectOfType<ItemSpawner>();

        if(spawner != null)
        {
            ItemSO item = spawner.PickUpItem();

            print(item);

            if(item != null)
            {
                inventory.AddItem(item);
            }
        }
    }

    public void PutItemInStorage()
    {
        //디버그용 함수로 가지고있는 모든 아이템을 배에 채웁니다

        ItemStorage storage = GameObject.FindObjectOfType<ItemStorage>();

        if(storage != null)
        {
            foreach(ItemSO item in inventory.itemList)
            {
                storage.AddItem(item);
            }
        }
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

            //UserVO vo = new UserVO(socketId, socketName, roomNum,transform.position,master);

            //string payload = JsonUtility.ToJson(vo);

            //DataVO dataVO = new DataVO("")
        }
    }

    public void SetTransform(Vector2 pos)
    {
        if(isRemote)
        {
            targetPos = pos;
        }
    }
}
