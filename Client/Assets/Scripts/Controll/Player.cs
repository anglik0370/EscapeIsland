using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;

    public int id;
    public bool isRemote;
    public Inventory inventory;
    public Color color;
    public Vector2 targetPos;

    public Vector2 moveDir = new Vector2();

    float xMove { get; set; }
    float yMove { get; set; }

    [SerializeField]
    private int speed = 5;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            PickUpNearlestItem();
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void InitPlayer()
    {

    }

    public void Move()
    {
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.y = Input.GetAxis("Vertical");

        transform.Translate(moveDir.normalized * speed * Time.deltaTime);
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

    IEnumerator SendPosition()
    {
        yield return null;
    }
}
