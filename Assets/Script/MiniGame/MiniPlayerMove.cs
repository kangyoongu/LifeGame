using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniPlayerMove : MonoBehaviour
{
    public float speed = 8;
    private Rigidbody2D rigid;

    public UnityEvent OnGameEnd;
    public UnityEvent OnGameClear;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        rigid.isKinematic = false;
    }
    void FixedUpdate()
    {
        if(MiniGameManager.Instance.gameStart)
        {
            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rigid.velocity = dir * speed;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            MiniGameManager.Instance.gameStart = false;
            OnGameEnd?.Invoke();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            MiniGameManager.Instance.gameStart = false;
            rigid.velocity = Vector2.zero;
            OnGameClear?.Invoke();
        }
        else if(collision.gameObject.name == "Cutline") { MiniGameManager.Instance.result = 1; }
    }
}
