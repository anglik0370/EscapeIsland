
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;

    public int id;
    public bool isRemote; //true : 다른놈 / false : 조작하는 플레이어
    public Inventory inventory;
    public Color color;
    public Vector2 targetPos;

    [SerializeField]
    public int speed = 5;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        if(!isRemote)
        {
            StartCoroutine(SendPosition());
        }
    }

    private void Update()
    {
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

    public void SetTransform(Vector2 pos)
    {
        if(isRemote)
        {
            targetPos = pos;
        }
    }
}
