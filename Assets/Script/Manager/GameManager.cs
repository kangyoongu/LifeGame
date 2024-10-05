using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.IO;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isPlaying = false;
    public GameObject dancer;
    public CinemachineVirtualCamera fadeinCam;
    public Material floorLine;

    public GameObject player;
    [HideInInspector]public Transform playerTrm;
    public Transform playerHead;

    private PlayerController playerController;
    public Camera mainCam;
    public PlayData difficulty;
    public GameObject[] miniGames;
    [SerializeField]public List<NPCcore> related;

    [HideInInspector] public bool haveLover = false;
    [HideInInspector] public bool haveWife = false;
    [HideInInspector] public bool haveBroked = false;
    [HideInInspector] public int girlFriendCount = 0;

    [HideInInspector] public int talkNum = -1;
    public TextMeshProUGUI dialogueText;

    public AudioSource aud;

    [SerializeField] private List<Image> checksEasy;
    [SerializeField] private List<Image> checksNormal;
    [SerializeField] private List<Image> checksHard;
    public Image startBlock;
    private EndingData endingData;
    public EndingData EndingData 
    { 
        get => endingData;
        set
        {
            endingData = value;
            Save(endingData);
            for(int i = 0; i < endingData.endingEasy.Length; i++)
            {
                checksEasy[i].color = endingData.endingEasy[i] ? Color.black : Color.white;
                checksNormal[i].color = endingData.endingNormal[i] ? Color.black : Color.white;
                checksHard[i].color = endingData.endingHard[i] ? Color.black : Color.white;
            }
        }
    }
    string path = "";
    void Awake()
    {
        if (Instance == null) Instance = this;
        playerTrm = player.transform;

        path = Path.Combine(Application.dataPath, "NeverOpenIt.json");
        if (File.Exists(path))
        {
            EndingData = Load();
        }
        else
        {
            EndingData = new EndingData();
        }
        floorLine.color = Color.black;
        startBlock.gameObject.SetActive(true);
        startBlock.DOFade(0, 2).OnComplete(() =>
        {
            startBlock.gameObject.SetActive(false);
        });
    }

    public EndingData Load()
    {
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<EndingData>(jsonData);
    }

    public void Save(EndingData data)
    {
        string jsonData = JsonUtility.ToJson(data, false);
        File.WriteAllText(path, jsonData);
    }

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if(talkNum != -1)
        {
            dialogueText.text = $"대화 {talkNum}번 남음";
        }
    }
    public void OnClickStart()
    {
        if (isPlaying == false)
        {
            UIManager.Instance.OutMainUI();
            fadeinCam.Priority = 0;
            floorLine.DOColor(Color.white, 2);
            dancer.transform.DOMoveY(-2.8f, 2).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                player.SetActive(true);
                player.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 2.8f).OnComplete(() =>
                {
                    playerController.enabled = true;
                });
            });
        }
    }
    public void EndMiniGame(int score)
    {
        for(int i = 0; i < miniGames.Length; i++)
            miniGames[i].SetActive(false);
        PlayMainGame();
        StartCoroutine(DelayFrame(score));
    }
    IEnumerator DelayFrame(int score)
    {
        yield return new WaitForSeconds(1);
        ProgressManager.Instance.subCam.Priority = 5;
        yield return new WaitForSeconds(2);
        ProgressManager.Instance.PrintWord(score);
    }
    public void StopMainGame()
    {
        mainCam.gameObject.SetActive(false);
    }
    public void PlayMainGame()
    {
        mainCam.gameObject.SetActive(true);
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
    public void Shoot()
    {
        aud.Play();
    }
}
