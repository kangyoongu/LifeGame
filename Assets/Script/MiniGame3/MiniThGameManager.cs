using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniThGameManager : MonoBehaviour
{
    public static MiniThGameManager Instance;
    [HideInInspector] public bool gameStart = false;

    public CinemachineVirtualCamera blackOutCam;
    public Transform player;
    public int result = 0;
    public UnityEvent OnGameStart;
    float time = 0;
    public UnityEvent OnGameClear;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void OnEnable()
    {
        time = 0;
        result = 0;
        StartCoroutine(Opening());
    }
    private void Update()
    {
        if(gameStart == true)
        {
            time += Time.deltaTime;
            if(time >= 8 && result == 0)
            {
                result = 1;
            }
            if(time >= 15)
            {
                gameStart = false;
                result = 2;
                OnGameClear?.Invoke();
                OnGameEnd();
            }
        }
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
