using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Thinfo
{
    public float Count;
    public float ObstacleSpeed;
    public float PlusSpeed;
}
public class MiniThObstacleSpawn : MonoBehaviour
{
    public Thinfo info;
    public static float speed;
    public Transform spawnPoint;
    void Awake()
    {
        info = GameManager.Instance.difficulty.Thdificult;
    }
    void OnEnable()
    {
        speed = info.ObstacleSpeed;
    }
    private void OnDisable()
    {
        ObjectPool.Instance.OffObject();
    }
    private void Update()
    {
        speed = Mathf.Min((speed + Time.deltaTime * info.PlusSpeed / 15), info.ObstacleSpeed + info.PlusSpeed);
    }
    public void SpawnStart()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        for (int i = 0; i < info.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);

            if (MiniThGameManager.Instance.gameStart)
            {
               ObjectPool.Instance.GetPooledObject("TrailObstacle", spawnPoint.position);
            }
        }
    }
}
