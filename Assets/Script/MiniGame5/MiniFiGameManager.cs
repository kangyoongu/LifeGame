using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class MiniFiGameManager : MonoBehaviour
{
    public static MiniFiGameManager Instance;
    [HideInInspector] public bool gameStart = false;

    public CinemachineVirtualCamera blackOutCam;
    public Transform player;
    public int result = 0;
    public UnityEvent OnGameStart;
    public UnityEvent OnStart;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void OnEnable()
    {
        player.localPosition = Vector3.zero;
        blackOutCam.transform.localPosition = new Vector3(0, -0.66f, -10);
        result = 0;
        OnStart?.Invoke();
        StartCoroutine(Opening());
    }
    public void OnDisable()
    {
        ObjectPool.Instance.OffObject();
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
