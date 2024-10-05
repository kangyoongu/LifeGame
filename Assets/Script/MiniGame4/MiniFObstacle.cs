using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFObstacle : MonoBehaviour
{
    bool move = false;
    private void OnEnable()
    {
        move = false;
        StopAllCoroutines();
        StartCoroutine(Manager());
    }
    private void Update()
    {
        if (move)
        {
            transform.Translate(Vector2.up * Time.deltaTime * MiniFSpawner.speed);//인포 스피드
        }
    }
    IEnumerator Manager()
    {
        yield return new WaitForSeconds(1);
        move = true;
        yield return new WaitForSeconds(5);
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
