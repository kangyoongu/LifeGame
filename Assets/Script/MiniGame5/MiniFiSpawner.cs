using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[Serializable]
public struct Fiinfo
{
    public float delayTime;
    public float minD;
    public float speed;
    public float maxS;
}
public class MiniFiSpawner : MonoBehaviour
{
    Fiinfo info;
    float time = 0;
    float d;
    public static float speed;
    float endTime = 0;
    bool spawn = true;
    public UnityEvent OnGameEnd;
    void Awake()
    {
        info = GameManager.Instance.difficulty.Fidificult;
    }
    void OnEnable()
    {
        d = info.delayTime;
        speed = info.speed;
        time = 0;
        endTime = 0;
        spawn = true;
    }
    void Update()
    {
        if (MiniFiGameManager.Instance.gameStart)
        {
            if (spawn)
            {
                time += Time.deltaTime;
                d = Mathf.Max((d - Time.deltaTime * info.minD / 18), info.delayTime - info.minD);
                speed = Mathf.Min((speed + Time.deltaTime * info.maxS / 18), info.speed + info.maxS);
                if (time >= d)
                {
                    Vector3 angle = Random.insideUnitCircle;
                    angle.Normalize();
                    ObjectPool.Instance.GetPooledObject("FiveObstarcle", MiniFiGameManager.Instance.player.position + angle * 60);
                    time = 0;
                }
            }
            endTime += Time.deltaTime;
            if (endTime >= 20)
            {
                spawn = false;
            }
            else if (endTime >= 12 && MiniFiGameManager.Instance.result == 0)
            {
                MiniFiGameManager.Instance.result = 1;
            }
            if (endTime >= 23)
            {
                MiniFiGameManager.Instance.result = 2;
                MiniFiGameManager.Instance.gameStart = false;
                OnGameEnd?.Invoke();
            }
        }
    }
}
