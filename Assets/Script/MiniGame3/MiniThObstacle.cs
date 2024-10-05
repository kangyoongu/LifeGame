using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniThObstacle : MonoBehaviour
{
    float rotate;
    Vector2 direction;
    private void OnEnable()
    {
        float size = Random.Range(0.7f, 1.2f);
        transform.localScale = new Vector2(size, size);
        rotate = Random.Range(100, 200) * (Random.value > 0.5f ? -1: 1);
        direction = Random.insideUnitCircle.normalized;
    }
    void Update()
    {
        if (MiniThGameManager.Instance.gameStart)
        {
            transform.Rotate(Vector3.forward * rotate * Time.deltaTime);
            transform.parent.position += (Vector3)direction * MiniThObstacleSpawn.speed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle") && !collision.gameObject.CompareTag("Player"))
        {
            Vector2 collisionPosition = collision.ClosestPoint(transform.position);

            // 현재 오브젝트의 위치
            Vector2 currentPosition = transform.position;

            // 입사각의 방향 벡터
            Vector2 incidentVector = collisionPosition - currentPosition;

            // 현재 방향과 입사각의 방향 벡터를 사용하여 반사각 계산
            float angle = Vector2.SignedAngle(direction, incidentVector);
            angle *= 2; // 반사각은 입사각과 같아야하므로 입사각 * 2

            // 반사각에 따라 방향을 조정
            direction = Quaternion.Euler(0, 0, angle) * -direction;
            transform.position += (Vector3)direction * MiniThObstacleSpawn.speed * Time.deltaTime;
            rotate *= -1;
        }
    }
}
