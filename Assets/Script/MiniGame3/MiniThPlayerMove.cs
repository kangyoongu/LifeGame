using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniThPlayerMove : MonoBehaviour
{
    public float speed = 8;
    public float jumpPoewr = 20;
    private Rigidbody2D rigid;
    public UnityEvent OnDie;
    public static bool canJump = false;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        transform.localPosition = Vector2.zero;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        rigid.isKinematic = true;
    }
    void FixedUpdate()
    {
        if (MiniThGameManager.Instance.gameStart)
        {
            float dir = Input.GetAxis("Horizontal");
            rigid.velocity = new Vector2(dir * speed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }
    private void Update()
    {
        if (MiniThGameManager.Instance.gameStart)
        {
            if (Input.GetKeyDown(KeyCode.W) && canJump)
            {
                rigid.AddForce(Vector2.up * jumpPoewr);
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            MiniThGameManager.Instance.gameStart = false;
            OnDie?.Invoke();
        }
    }
}
