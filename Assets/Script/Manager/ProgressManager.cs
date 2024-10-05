using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;
    public Transform startPoint;
    [HideInInspector]public Transform[] startPoints;
    public CinemachineVirtualCamera[] mainCam;
    public CinemachineVirtualCamera subCam;
    public PlayData[] stateInfo;
    public string difficulty;
    [SerializeField] bool typinging;
    Node node;

    string nowName;//NPC정보들
    public delegate float FloatDelegate(float value);
    FloatDelegate nowInteraction;
    Scenario nowScenario;
    Vector2 nowRange;
    int nowState;
    MoveNPCCore nowNpc;
    int result;
    Action nowFarewell;

    public Action interaction;
    public Action interaction2;
    public GameObject town;
    public GameObject school;
    [SerializeField]private GameObject manPref;
    [SerializeField]private GameObject womanPref;

    Coroutine c;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        startPoints = startPoint.GetComponentsInChildren<Transform>();
        typinging = false;
    }
    public void OpeningOn()
    {
        StartCoroutine(UIManager.Instance.GameOpening());
    }
    public void GoTown()//마을로 넘어감
    {
        BGMManager.Instance.FadeInAndOut(2, 1.3f, 1.3f);
        town.SetActive(true);
        EndingManager.Instance.startFriend = GameManager.Instance.related.Count;
        if (GameManager.Instance.related.Count > 0)
        {
            for (int i = 0; i < GameManager.Instance.related.Count; i++)
            {
                MoveNPCCore npc = Instantiate(GameManager.Instance.related[i].gender ? manPref : womanPref,
                    TownPointsList.Instance.points[Random.Range(0, TownPointsList.Instance.length)].position, Quaternion.identity).GetComponent<MoveNPCCore>();//npc 성별에 맞게 생성
                npc.Set(GameManager.Instance.related[i]._name, GameManager.Instance.related[i].love, GameManager.Instance.related[i].Friend);//값 할당
            }
        }
        StartCoroutine(UIManager.Instance.GoTown());
    }
    public void OnClickDifficulty(int difficulty)//난이도 선택 버튼
    {
        GameManager.Instance.difficulty = stateInfo[difficulty];
        GameManager.Instance.talkNum = stateInfo[difficulty].talkNum;
        BGMManager.Instance.FadeInAndOut(1, 1.3f, 1.3f);
        switch (difficulty)
        {
            case 0:
                this.difficulty = "Easy";
                break;
            case 1:
                this.difficulty = "Normal";
                break;
            case 2:
                this.difficulty = "Hard";
                break;
        }
        StartCoroutine(UIManager.Instance.GameOpeningOff());
        UIManager.Instance.InPlayUI();
    }
    public void OffInteraction()
    {
        interaction?.Invoke();
    }
    public void OffInteraction2()
    {
        interaction2?.Invoke();
    }
    public void ProgressScenario(int scenario, string name, FloatDelegate inter, Action farewell)//대화 시작하면
    {
        nowInteraction = inter;
        nowName = name;//이름 가져오고
        nowScenario = ScenarioMaker.Instance.scenarios[scenario];
        nowState = scenario;
        nowFarewell = farewell;
        subCam.Priority = 15;
        StartCoroutine(Delay());
    }
    public void ProgressScenario(int scenario, string name, FloatDelegate inter, Action farewell, MoveNPCCore npccore)//대화 시작하면
    {
        nowInteraction = inter;
        nowName = name;//이름 가져오고
        nowScenario = ScenarioMaker.Instance.scenarios[scenario];
        nowState = scenario;
        nowFarewell = farewell;
        nowNpc = npccore;
        subCam.Priority = 15;
        StartCoroutine(Delay());
    }
    void ProgressScenario(Node scenario)//대화 진행
    {
        UIManager.Instance.nameText.text = scenario.me ? "나" : nowName;

        node = scenario;
        if(c != null) StopCoroutine(c);
        c = StartCoroutine(TypeText());//대화 내용 출력
        #region ff
        /*
                else if (scenario.Count > 1)//선택지 있으면
                {
                    if (scenario[0].me == true)
                    {
                        UIManager.Instance.scenario.text = "";
                        List<RectTransform> selection = new List<RectTransform>();
                        UIManager.Instance.skipBut.SetActive(false);
                        selection.Add(UIManager.Instance.selectionWindow);
                        selection[0].GetChild(0).GetComponent<TextMeshProUGUI>().text = scenario[0].dialogue;

                        for (int i = 1; i < scenario.Count; i++)
                        {
                            selection.Add(Instantiate(selection[0].gameObject, Vector2.zero, Quaternion.identity).GetComponent<RectTransform>());//선택지 만들기
                            selection[i].parent = selection[0].parent;
                            selection[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = scenario[i].dialogue;
                            selection[i].DOScale(Vector2.one, 1);
                        }
                        selection[0].DOScale(Vector2.one, 1);

                        for (int j = 0; j < scenario.Count; j++)//선택지 버튼에 함수 넣기
                        {
                            int f = j;
                            selection[j].GetComponent<Button>().onClick.AddListener(() => StartCoroutine(OnClickSelection(scenario[f], selection)));
                        }
                    }
                    else
                    {
                        foreach(Node s in scenario)
                        {
                            if(s.printRange.x <= nowHelf && s.printRange.y >= nowHelf)
                            {
                                ProgressScenario(new List<Node> { s });
                                break;
                            }
                        }
                    }
                }*/
        #endregion
    }/*
    IEnumerator OnClickSelection(Node node, List<RectTransform> select)
    {
        for(int i = 0; i < select.Count; i++)
        {
            select[i].DOScale(Vector2.zero, 1);
        }
        yield return new WaitForSeconds(1);
        for (int i = 1; i < select.Count; i++)
        {
            Destroy(select[i].gameObject);
            Destroy(select[i]);
        }
        UIManager.Instance.skipBut.SetActive(true);
        ProgressScenario(node.nextNode);
    }*/
    IEnumerator TypeText()
    {
        typinging = true;
        UIManager.Instance.scenario.text = "";
        string wording = node.dialogue.Replace("fullname", nowName).Replace("name", nowName.Substring(1));
        foreach (char letter in wording.ToCharArray())
        {
            UIManager.Instance.scenario.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        typinging = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(UIManager.Instance.textWindow.GetComponent<RectTransform>().localScale.x == 1)
            {
                //OnClickNext();
            }
        }
    }
    public void OnClickNext()
    {
        if (typinging == true)
        {
            if (c != null) StopCoroutine(c);
            UIManager.Instance.scenario.text = node.dialogue.Replace("fullname", nowName).Replace("name", nowName.Substring(1));
            typinging = false;
        }
        else
        {
            if (node.nextNode.Count != 0)//다음대사가 있으면
            {
                ProgressScenario(node.nextNode[0]);
            }
            else//끝
            {
                float? a = nowInteraction?.Invoke(Random.Range(nowRange.x, nowRange.y));//호감도를 return
                if (nowState == 2 && a <= 0 && result == 0)
                {
                    nowState = 3;
                    ProgressScenario(ScenarioMaker.Instance.scenarios[3].node[Random.Range(0, ScenarioMaker.Instance.scenarios[3].node.Count)].node);
                }
                else if(nowState == 5 && a <= -50 && result == 0)//이혼 대화창 실행============
                {
                    nowState = 6;
                    ProgressScenario(ScenarioMaker.Instance.scenarios[6].node[Random.Range(0, ScenarioMaker.Instance.scenarios[6].node.Count)].node);
                }
                else//만약 끝나서 창 닫아야하는 상태면
                {
                    if (nowState == 3 || nowState == 6)//이별
                    {
                        if(nowState == 6)//진짜 여기서 이혼함======================================
                        {
                            GameManager.Instance.haveBroked = true;
                        }
                        nowFarewell?.Invoke();
                    }
                    else if (nowState == 1 && result == 2)//고백성공
                    {
                        nowInteraction?.Invoke(1024);
                        GameManager.Instance.girlFriendCount++;
                    }
                    else if (nowState == 4 && result == 2)//청혼성공
                    {
                        nowInteraction?.Invoke(3072);
                    }
                    /*else if (nowState == 4 && result <= 1)//청혼거절
                    {
                        nowInteraction?.Invoke(4096);
                    }*/
                    if (nowState == 2 && result == 2)//여친한테 좋은말
                    {
                        nowInteraction?.Invoke(2000);
                    }
                    else if (nowState == 2 && result <= 1)//여친한테 실언
                    {
                        nowInteraction?.Invoke(3000);
                    }
                    if (nowNpc)
                    {
                        if (a <= -100 && result <= 0)
                        {
                            nowNpc.disputeCount++;
                        }
                        else
                        {
                            nowNpc.disputeCount = 0;
                        }
                    }
                    if (nowNpc && nowNpc.disputeCount >= 3 && nowState != 7)
                    {
                        nowState = 7;
                        ProgressScenario(ScenarioMaker.Instance.scenarios[7].node[Random.Range(0, ScenarioMaker.Instance.scenarios[7].node.Count)].node);
                    }
                    else
                    {
                        UIManager.Instance.textWindow.GetComponent<RectTransform>().DOScale(Vector2.zero, 1).OnComplete(() =>
                        {
                            if (nowState == 7)
                            {
                                nowNpc.KillPlayer();
                            }
                            else if(GameManager.Instance.talkNum <= 0)
                            {                                
                                EndingManager.Instance.EndEnding();
                            }
                        });//대화창 닫고
                        UIManager.Instance.scenario.text = "";//글자 비우기
                        if (nowState != 7 && GameManager.Instance.talkNum > 0)
                        {
                            if (nowNpc)
                            {
                                UIManager.Instance.MapIn(1);
                            }
                            PlayerController.canMove = true;
                            nowInteraction?.Invoke(2048);
                        }
                    }
                }
            }
        }
    }

    public void GameStart()//미니게임 시작할 때 카메라 등으로 옮기고
    {
        subCam.Priority = 15;
        StartCoroutine(Delay());
    }
    IEnumerator Delay()//다 옮겨지면 씬 옮겨서 시작
    {
        yield return new WaitForSeconds(3);
        GameManager.Instance.StopMainGame();
        GameManager.Instance.miniGames[Random.Range(0, GameManager.Instance.miniGames.Length)].SetActive(true);
    }

    public void PrintWord(int index)//미니게임 끝나고 결과 오면 그거에 따라 대화 진행
    {
        result = index;
        List<NodeRoot> lst = new List<NodeRoot>();
        foreach (NodeRoot node in nowScenario.node)
        {
            if (node.result == index) lst.Add(node);
        }
        int ind = Random.Range(0, lst.Count);
        nowRange = lst[ind].friend;
        node = lst[ind].node;

        UIManager.Instance.nameText.text = node.me ? "나" : nowName;
        UIManager.Instance.textWindow.GetComponent<RectTransform>().DOScale(Vector2.one, 1).OnComplete(()=>
        {
            ProgressScenario(node);
        });//대화창 열기
    }
}
