using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFiObstacle : MonoBehaviour
{
    private void OnEnable()
    {
        if (MiniFiGameManager.Instance)
        {
            float scale = Random.Range(1.5f, 3f);
            transform.GetChild(0).localScale = new Vector3(scale, scale, 1);
            Vector3 direction = MiniFiGameManager.Instance.player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            StartCoroutine(End());
        }
    }
    void Update()
    {
        if (MiniFiGameManager.Instance.gameStart)
        {
            transform.Translate(Vector2.up * MiniFiSpawner.speed * Time.deltaTime);
        }
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(7);
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
