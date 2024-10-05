using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTObstacle : MonoBehaviour
{
    float ObstaSpeed;
    float rotateSpeed;
    Vector3 dir;
    Transform child;
    public void Set(float speed, float scale, float rotate, Vector3 target)
    {
        ObstaSpeed = speed; rotateSpeed = rotate;
        transform.GetChild(0).localScale = new Vector3(scale, scale, 1);
        dir = target - transform.position;
        child = transform.GetChild(0);
        dir.Normalize();
    }
    void Update()
    {
        if (MiniTGameManager.Instance.gameStart)
        {
            transform.position += dir * ObstaSpeed * Time.deltaTime;
            child.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            if (Vector2.SqrMagnitude(MiniTGameManager.Instance.player.position-transform.position) > 2000)
            {
                ObjectPool.Instance.ReturnToPool(gameObject);
            }
        }
    }
}
