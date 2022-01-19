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
    public int speed = 5;

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
        
        if(Input.GetKeyDown(KeyCode.E))
        {
            PutItemInStorage();
        }
    }

    public void InitPlayer()
    {

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

    IEnumerator SendPosition()
    {
        yield return null;
    }
}
