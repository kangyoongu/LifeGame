using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniObstacle : MonoBehaviour
{
    float ObstaSpeed;
    float rotateSpeed;

    public void Set(float speed, float scale, float rotate)
    {
        ObstaSpeed = speed; rotateSpeed = rotate;
        transform.localScale = new Vector3(scale, scale, 1);
    }
    void Update()
    {
        if (MiniGameManager.Instance.gameStart)
        {
            transform.position += Vector3.down * ObstaSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            if (transform.position.y < MiniGameManager.Instance.player.position.y - 30)
            {
                ObjectPool.Instance.ReturnToPool(gameObject);
            }
        }
    }
}
