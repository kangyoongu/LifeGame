using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class MiniTGameManager : MonoBehaviour
{
    public static MiniTGameManager Instance;
    public Transform player;
    public bool gameStart = false;
    public CinemachineVirtualCamera opening;
    public UnityEvent OnStart;
    public int result = 0;
    public UnityEvent OnThisEnable;
    private void Awake()
    {
        if (Instance == null) Instance = this; 
    }
    private void OnEnable()
    {
        OnThisEnable?.Invoke();
        result = 0;
        opening.transform.localPosition = new Vector3(0, -1, -10);
        StartCoroutine(CamMoveEnd());
    }
    private void OnDisable()
    {
        ObjectPool.Instance.OffObject();
    }
    IEnumerator CamMoveEnd()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        opening.Priority = 20;
        yield return new WaitForSeconds(3);
        OnStart?.Invoke();
        gameStart = true;
    }
    public void Die()
    {
        opening.Priority = 30;
        gameStart = false;
        StartCoroutine(Endgame());
    }
    IEnumerator Endgame()
    {
        yield return new WaitForSeconds(3);
        GameManager.Instance.EndMiniGame(result);
    }
}
