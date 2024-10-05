using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class MiniFGameManager : MonoBehaviour
{
    public static MiniFGameManager Instance;
    [HideInInspector] public bool gameStart = false;

    public CinemachineVirtualCamera blackOutCam;
    public Transform player;
    public int result = 0;
    public UnityEvent OnGameStart;
    float time = 0;
    public UnityEvent OnGameClear;
    public UnityEvent OnStart;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void OnEnable()
    {
        time = 0;
        result = 0;
        OnStart?.Invoke();
        StartCoroutine(Opening());
    }
    
    IEnumerator Opening()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        blackOutCam.Priority = 20;
        yield return new WaitForSeconds(3.5f);
        OnGameStart?.Invoke();
        gameStart = true;
    }
    public void OnDisable()
    {
        ObjectPool.Instance.OffObject();
    }
    private void Update()
    {
        if (gameStart == true)
        {
            time += Time.deltaTime;
            if (time >= 13 && result == 0)
            {
                result = 1;
            }
            if (time >= 20)
            {
                MiniFSpawner.spawn = false;
                result = 2;
            }
            if (time >= 22)
            {
                gameStart = false;
                OnGameClear?.Invoke();
                OnGameEnd();
            }
        }
    }
    public void OnGameEnd()
    {
        StartCoroutine(GameEnd());
    }
    public IEnumerator GameEnd()
    {
        blackOutCam.Priority = 30;
        yield return new WaitForSeconds(3);
        GameManager.Instance.EndMiniGame(result);
    }
}
