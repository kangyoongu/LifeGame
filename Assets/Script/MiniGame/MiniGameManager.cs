using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;
    public Transform player;
    public Transform bottom;
    public CinemachineVirtualCamera cinemachine;
    public bool gameStart = false;
    public int result;
    Tweener b;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void OnEnable()
    {
        StartCoroutine(GameStart());
        result = 0;
    }
    private void OnDisable()
    {
        player.transform.position = new Vector2(-424.4f, -5f);
        bottom.transform.position = new Vector2(-424.4f, -32.54f);
        player.GetChild(0).gameObject.SetActive(true);
        ObjectPool.Instance.OffObject();
    }
    IEnumerator GameStart()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        float currentOrthoSize = 0.1f;
        DOTween.To(() => currentOrthoSize, x => currentOrthoSize = x, 5, 1).SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                cinemachine.m_Lens.OrthographicSize = currentOrthoSize;
            });
        yield return new WaitForSeconds(0.9f);
        cinemachine.Priority = 19;
        yield return new WaitForSeconds(3);
        gameStart = true;
        b = bottom.DOMoveY(150.6f, 30).SetEase(Ease.InSine);
    }
    public void StopBottom()
    {
        b.Kill();
        StartCoroutine(GameEnd());
    }
    IEnumerator GameEnd()
    {
        cinemachine.Priority = 25;
        yield return new WaitForSeconds(2.9f);
        float currentOrthoSize = 5f;
        DOTween.To(() => currentOrthoSize, x => currentOrthoSize = x, 0.1f, 2).SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                cinemachine.m_Lens.OrthographicSize = currentOrthoSize;
            }).OnComplete(() =>
            {
                GameManager.Instance.EndMiniGame(result);//씬 넘어가는 함수 실행 나중에 난이도 받아서 그거
            });
    }
    public void Clear()
    {
        result = 2;
    }
}
