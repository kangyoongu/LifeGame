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

            // ���� ������Ʈ�� ��ġ
            Vector2 currentPosition = transform.position;

            // �Ի簢�� ���� ����
            Vector2 incidentVector = collisionPosition - currentPosition;

            // ���� ����� �Ի簢�� ���� ���͸� ����Ͽ� �ݻ簢 ���
            float angle = Vector2.SignedAngle(direction, incidentVector);
            angle *= 2; // �ݻ簢�� �Ի簢�� ���ƾ��ϹǷ� �Ի簢 * 2

            // �ݻ簢�� ���� ������ ����
            direction = Quaternion.Euler(0, 0, angle) * -direction;
            transform.position += (Vector3)direction * MiniThObstacleSpawn.speed * Time.deltaTime;
            rotate *= -1;
        }
    }
}
