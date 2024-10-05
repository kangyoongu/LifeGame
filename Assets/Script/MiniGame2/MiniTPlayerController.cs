using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniTPlayerController : MonoBehaviour
{
    public float speed = 8;
    private Rigidbody2D rigid;
    public UnityEvent OnDie;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        transform.localPosition = Vector2.zero;
        rigid.isKinematic = false;
    }
    void FixedUpdate()
    {
        if (MiniTGameManager.Instance.gameStart)
        {
            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rigid.velocity = dir * speed;
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            MiniTGameManager.Instance.opening.transform.position = collision.transform.position + new Vector3(0, 0, -10);
            OnDie?.Invoke();
        }
    }
}
