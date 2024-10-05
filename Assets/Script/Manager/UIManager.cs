using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public RectTransform[] main;
    public RectTransform[] play;
    public Vector2[] mainPos;
    public Vector2[] playPos;
    public Image[] board;
    public GameObject inBut;
    public GameObject cvs;

    public GameObject textWindow;
    [HideInInspector] public TextMeshProUGUI nameText;
    [HideInInspector] public TextMeshProUGUI scenario;
    [HideInInspector] public GameObject skipBut;

    public GameObject ending;
    public Image endingBackground;
    public TextMeshProUGUI endingName;
    public TextMeshProUGUI endingExp;
    public Image endingImage;
    public Image empty;
    public RectTransform[] townUI;
    public Vector2[] townPos;

    public RectTransform setting;
    Tweener settingTweener;
    public GameObject block;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        nameText = textWindow.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        scenario = textWindow.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        skipBut = textWindow.transform.GetChild(2).gameObject;
    }
    public void OutMainUI()
    {
        block.SetActive(true);
        for (int i = 0; i < main.Length; i++)
        {
            main[i].DOAnchorPosX(mainPos[i].x, 1).SetEase(Ease.OutCubic);
        }
    }
    public void InMainUI()
    {
        main[0].DOAnchorPosX(mainPos[0].y, 1).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            block.SetActive(false);
        });
        for (int i = 1; i < main.Length; i++)
        {
            main[i].DOAnchorPosX(mainPos[i].y, 1).SetEase(Ease.OutCubic);
        }
    }
    public void OutPlayUI()
    {
        play[0].DOAnchorPosY(playPos[0].y, 1).SetEase(Ease.OutCubic);
    }
    public void InPlayUI()
    {
        play[0].DOAnchorPosY(playPos[0].x, 1).SetEase(Ease.OutCubic);
    }
    public void OnClickCut()
    {
        SceneManager.LoadScene(0);
    }
    public IEnumerator GameOpening()
    {
        board[0].DOFade(1, 2);
        yield return new WaitForSeconds(3);

        board[1].gameObject.SetActive(true);
        board[2].gameObject.SetActive(true);
        board[2].DOFade(0, 2).SetEase(Ease.InCirc);

        yield return new WaitForSeconds(2);
        board[2].gameObject.SetActive(false);
    }
    public IEnumerator GameOpeningOff()
    {
        board[2].gameObject.SetActive(true);
        board[2].DOFade(1, 1).SetEase(Ease.OutCirc);
        GameManager.Instance.playerTrm.position = ProgressManager.Instance.startPoints[1].position;
        ProgressManager.Instance.mainCam[0].Priority = 5;
        ProgressManager.Instance.mainCam[1].Priority = 10;

        yield return new WaitForSeconds(2);
        board[2].gameObject.SetActive(false);
        board[1].gameObject.SetActive(false);
        board[0].DOFade(0, 1).SetEase(Ease.InCirc);

        yield return new WaitForSeconds(2);
        PlayerController.canMove = true;
        GameManager.Instance.isPlaying = true;
    }
    public IEnumerator GoTown()
    {
        board[0].DOFade(1, 2);
        yield return new WaitForSeconds(2);

        GameManager.Instance.playerTrm.position = ProgressManager.Instance.startPoints[2].position;
        ProgressManager.Instance.school.SetActive(false);

        yield return new WaitForSeconds(2);
        board[0].DOFade(0, 2);
        TownUIIn(2);
        yield return new WaitForSeconds(2);
        PlayerController.canMove = true;
    }
    public IEnumerator EndingUIOn(EndingSet endingSet, float fadeIn, float firstDelay)
    {
        yield return new WaitForSeconds(firstDelay);
        empty.DOFade(1, fadeIn).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(fadeIn);
        ending.SetActive(true);
        endingName.text = endingSet.title;
        endingExp.text = endingSet.explain;
        endingImage.sprite = endingSet.image;
        BGMManager.Instance.FadeInAndOut(3, 1.3f, 1.3f);
        yield return new WaitForSeconds(3);
        endingBackground.DOFade(0, 3).SetEase(Ease.InCirc).OnComplete(() => 
        {
            endingBackground.gameObject.SetActive(false);
        });
    }
    public void TownUIIn(float time)
    {
        townUI[0].DOAnchorPosY(townPos[0].x, time);
        townUI[1].DOAnchorPosY(townPos[1].x, time);
    }
    public void TownUIOut(float time)
    {
        townUI[0].DOAnchorPosY(townPos[0].y, time);
        townUI[1].DOAnchorPosY(townPos[1].y, time);
    }
    public void MapIn(float time)
    {
        townUI[0].DOAnchorPosY(townPos[0].x, time);
        townUI[1].DOAnchorPosY(townPos[1].x, time);
    }
    public void MapOut(float time)
    {
        townUI[0].DOAnchorPosY(-63, time);
        townUI[1].DOAnchorPosY(townPos[1].y, time);
    }
    public void OnClickSetting()
    {
        if(settingTweener == null || !settingTweener.IsPlaying()){
            settingTweener = setting.DOAnchorPosY(0, 1).SetEase(Ease.OutCubic);
            OutMainUI();
        }
    }
    public void OnClickSettingClose()
    {
        if (settingTweener == null || !settingTweener.IsPlaying())
        {
            settingTweener = setting.DOAnchorPosY(-962, 1).SetEase(Ease.OutCubic);
            InMainUI();
        }
    }
    public void OnClickMain()
    {
        endingBackground.gameObject.SetActive(true);
        BGMManager.Instance.FadeOut(1.3f);
        endingBackground.DOFade(1, 2).OnComplete(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
