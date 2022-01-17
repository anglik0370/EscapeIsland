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
    }

    private void Update()
    {
        
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

    IEnumerator SendPosition()
    {
        yield return null;
    }
}
