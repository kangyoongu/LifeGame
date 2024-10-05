using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Ending : short
{
    Nothing,
    OneFriend,
    Marry,
    BreakWife,
    ReMarry,
    Lovelace,
    ManyFriend,
    Loser,
    Loneliness,
    LonelinessDie,
    Murder,
}

[Serializable]
public struct EndingSet
{
    public Ending ending;
    public string title;
    public string explain;
    public Sprite image;
}


public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;
    public PlayerController playerController;
    public EndingSet[] endingInfo;

    [HideInInspector] public float stopTime = 0;
    [HideInInspector] public float time = 0;
    [HideInInspector] public float startFriend = -1;
    [HideInInspector] public float endFriend = 0;

    public bool manyFriend = false;
    public bool oneFriend = false;
    public bool manyLove = false;
    public bool marry = false;
    public bool breakWife = false;
    public bool reMarry = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Update()
    {
        if(endFriend == startFriend && GameManager.Instance.isPlaying)
        {
            stopTime += Time.deltaTime;
            time += Time.deltaTime;
            if(stopTime >= 15)//멀어지는 속도
            {
                playerController.KillPlayer();
            }
            if(time >= 100)
            {
                EndingFunc(Ending.Loneliness, 4, 0, true);
            }
        }
        if(startFriend >= 10 && GameManager.Instance.isPlaying)
        {
            if(endFriend == startFriend)
            {
                EndingFunc(Ending.Loser, 2, 1, true);
            }
        }
    }
    public void EndingFunc(Ending ending, float fadeIn, float firstDelay, bool moveStop = false)
    {
        UIManager.Instance.OutPlayUI();
        ProgressManager.Instance.OffInteraction2();
        UIManager.Instance.TownUIOut(1);

        GameManager.Instance.isPlaying = false;
        PlayerController.canMove = moveStop;

        if (ProgressManager.Instance.difficulty == "Easy")
        {
            EndingData ed = GameManager.Instance.EndingData;
            ed.endingEasy[(short)ending] = true;
            GameManager.Instance.EndingData = ed;
        }
        else if (ProgressManager.Instance.difficulty == "Normal")
        {
            EndingData ed = GameManager.Instance.EndingData;
            ed.endingNormal[(short)ending] = true;
            GameManager.Instance.EndingData = ed;
        }
        else if (ProgressManager.Instance.difficulty == "Hard")
        {
            EndingData ed = GameManager.Instance.EndingData;
            ed.endingNormal[(short)ending] = true;
            GameManager.Instance.EndingData = ed;
        }

        StartCoroutine(UIManager.Instance.EndingUIOn(endingInfo[(short)ending], fadeIn, firstDelay));
    }
    public void EndEnding()
    {
        if (startFriend == 1 && endFriend == 0) oneFriend = true;
        else if (startFriend - endFriend >= 15) manyFriend = true;
        if (GameManager.Instance.haveWife == true)
        {
            if (GameManager.Instance.haveBroked) reMarry = true;
            else marry = true;
        }
        else if (GameManager.Instance.haveBroked) breakWife = true;
        if (GameManager.Instance.girlFriendCount >= 5) manyLove = true;

        if (manyFriend)
        {
            EndingFunc(Ending.ManyFriend, 4, 0, true);
        }
        else if (manyLove)
        {
            EndingFunc(Ending.Lovelace, 4, 0, true);
        }
        else if (reMarry)
        {
            EndingFunc(Ending.ReMarry, 4, 0, true);
        }
        else if (breakWife)
        {
            EndingFunc(Ending.BreakWife, 4, 0, true);
        }
        else if (marry)
        {
            EndingFunc(Ending.Marry, 4, 0, true);
        }
        else if (oneFriend)
        {
            EndingFunc(Ending.OneFriend, 4, 0, true);
        }
        else
        {
            EndingFunc(Ending.Nothing, 4, 0, true);
        }
    }
}
