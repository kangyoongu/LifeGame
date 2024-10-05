using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniFPlayerMove : MonoBehaviour
{
    public UnityEvent OnDie;
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
    void Update()
    {
        if (MiniFGameManager.Instance.gameStart)
        {
            if (Input.GetKeyDown(KeyCode.W) && transform.localPosition.y < 5)
            {
                transform.position += new Vector3(0, 3.0072f, 0);
            }
            if (Input.GetKeyDown(KeyCode.S) && transform.localPosition.y > -5)
            {
                transform.position += new Vector3(0, -3.0072f, 0);
            }
            if (Input.GetKeyDown(KeyCode.A) && transform.localPosition.x > -5)
            {
                transform.position += new Vector3(-3.0072f, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.D) && transform.localPosition.x < 5)
            {
                transform.position += new Vector3(3.0072f, 0, 0);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            OnDie?.Invoke();
            MiniFGameManager.Instance.gameStart = false;
        }
    }
}
