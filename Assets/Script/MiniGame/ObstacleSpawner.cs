using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Dificult
{
    public float bigObstaDelay;
    public float smallObstaDelay;
    public float bigObstaMinD;//20초동안 줄어든다.
    public float smallObstaMinD;
    public float bigObstaSpeed;
    public float smallObstaSpeed;
    public float bigObstaScale;
    public float smallObstaScale;
}
public class ObstacleSpawner : MonoBehaviour
{
    public Dificult info;
    float bigTime = 0;
    float smallTime = 0;
    float sd;
    float bd;
    void Awake()
    {
        info = GameManager.Instance.difficulty.dificult;
    }
    void OnEnable()
    {
        sd = info.smallObstaDelay;
        bd = info.bigObstaDelay;
        bigTime = 0;
        smallTime = 0;
    }
    void Update()
    {
        if (MiniGameManager.Instance.gameStart)
        {
            bigTime += Time.deltaTime;
            smallTime += Time.deltaTime;
            bd = Mathf.Max((bd - Time.deltaTime * info.bigObstaMinD / 30), info.bigObstaDelay - info.bigObstaMinD);
            sd = Mathf.Max((sd - Time.deltaTime * info.smallObstaMinD / 30), info.smallObstaDelay - info.smallObstaMinD);
            if (bigTime >= bd)
            {
                short dir = (short)(Random.value > 0.5f ? 1 : -1);
                ObjectPool.Instance.GetPooledObject("BigObstarcle", new Vector2(Random.Range(3.5f, 11f) * dir + MiniGameManager.Instance.bottom.position.x, MiniGameManager.Instance.player.position.y + 70))
                    .GetComponent<MiniObstacle>().Set(info.bigObstaSpeed + Random.Range(-1f, 1f), info.bigObstaScale + Random.Range(-2.5f, 2.5f), Random.Range(20f, 40f) * -dir);
                bigTime = 0;
            }
            if (smallTime >= sd)
            {
                ObjectPool.Instance.GetPooledObject("SmallObstarcle", new Vector2(Random.Range(-8f, 8f) + MiniGameManager.Instance.bottom.position.x, MiniGameManager.Instance.player.position.y + 70))
                    .GetComponent<MiniObstacle>().Set(info.smallObstaSpeed + Random.Range(-2f, 2f), info.smallObstaScale + Random.Range(-0.9f, 0.9f), Random.Range(80f, 130f) * (Random.value > 0.5f ? 1 : -1));
                smallTime = 0;
            }
        }
    }
}
