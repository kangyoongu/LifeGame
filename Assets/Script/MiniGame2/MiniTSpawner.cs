using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[Serializable]
public struct Tinfo
{
    public float delayTime;
    public float minD;
    public float scale;
    public float speed;
    public float maxS;
    public float distinct;
    public float safezoneSpeed;
}
public class MiniTSpawner : MonoBehaviour
{
    Tinfo info;
    float time = 0;
    float d;
    float speed;
    public Transform safeZone;
    float endTime = 0;
    bool spawn = true;
    public UnityEvent OnGameEnd;
    GameObject obstacle;
    Tweener safe;
    void Awake()
    {
        info = GameManager.Instance.difficulty.Tdificult;
    }
    void OnEnable()
    {
        d = info.delayTime;
        speed = info.speed;
        time = 0;
        endTime = 0;
        spawn = true;
        safeZone.localPosition = Vector3.zero;
        if (safe != null) safe.Kill();
    }
    void Update()
    {
        if (MiniTGameManager.Instance.gameStart)
        {
            if (spawn)
            {
                time += Time.deltaTime;
                d = Mathf.Max((d - Time.deltaTime * info.minD / 10), info.delayTime - info.minD);
                speed = Mathf.Min((speed + Time.deltaTime * info.maxS / 10), info.speed + info.maxS);
                if (time >= d)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        short dir = (short)(Random.value > 0.5f ? 1 : -1);
                        Vector3 angle = Random.insideUnitCircle;
                        angle.Normalize();
                        obstacle = ObjectPool.Instance.GetPooledObject("Obstarcle", MiniTGameManager.Instance.player.position + (angle * 30));
                        obstacle.GetComponent<MiniTObstacle>().Set(speed + Random.Range(-3f, 3f), info.scale + Random.Range(-0.2f, 0.2f), Random.Range(900f, 150f) * -dir,
                            safeZone.position + Quaternion.Euler(0, 0, (Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg + (90 * (short)(Random.value > 0.5f ? 1 : -1)))) * Vector3.right * Random.Range(info.distinct, 10));
                    }
                    time = 0;
                }
            }
            endTime += Time.deltaTime;
            if(endTime >= 11)
            {
                spawn = false;
            }
            else if(endTime >= 6 && MiniTGameManager.Instance.result == 0)
            {
                MiniTGameManager.Instance.result = 1;
            }
            if(obstacle && obstacle.activeSelf == false && spawn == false)
            {
                MiniTGameManager.Instance.result = 2;
                MiniTGameManager.Instance.gameStart = false;
                OnGameEnd?.Invoke();
            }
        }
    }
    public void MoveSafeZone()
    {
        float dis = Random.Range(0.5f, 3f);
        safe = safeZone.DOMove(safeZone.position + (Vector3)(Random.insideUnitCircle.normalized * dis), dis * info.safezoneSpeed).OnComplete(() => 
        {
            MoveSafeZone();
        });
    }
}
