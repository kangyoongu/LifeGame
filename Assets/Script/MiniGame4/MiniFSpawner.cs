using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[Serializable]
public struct Finfo
{
    public float delayTime;
    public float minDelay;
    public float speed;
    public float plusS;
}
public class MiniFSpawner : MonoBehaviour
{
    Finfo info;
    float time = 0;
    float d;
    public static float speed;
    public Transform[] points;
    public static bool spawn = true;
    void Awake()
    {
        info = GameManager.Instance.difficulty.Fdificult;
    }
    void OnEnable()
    {
        spawn = true;
        d = info.delayTime;
        speed = info.speed;
        time = 0;
    }
    void Update()
    {
        if (MiniFGameManager.Instance.gameStart && spawn)
        {
            time += Time.deltaTime;
            d = Mathf.Max((d - Time.deltaTime * info.minDelay / 20), info.delayTime - info.minDelay);
            speed = Mathf.Max((speed + Time.deltaTime * info.plusS / 20), info.speed + info.plusS);
            if (time >= d)
            {
                int index = Random.Range(0, points.Length);
                GameObject g = ObjectPool.Instance.GetPooledObject("FourObstarcle", points[index].position);
                g.transform.rotation = points[index].rotation;
                time = 0;
            }
        }
    }
}
