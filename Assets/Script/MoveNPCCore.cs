using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class MoveNPCCore : MonoBehaviour, INpc
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    int index;

    string _name = "";
    bool gender = false;//true���� false ����
    [HideInInspector] public Transform neck;
    float rotationSpeed = 4f; // ȸ�� �ӵ��� �����մϴ�.
    Transform head;
    [HideInInspector] public bool look = false;

    private Transform inBut;//��ȣ�ۿ��ư
    [HideInInspector] public List<SkinnedMeshRenderer> materials = new List<SkinnedMeshRenderer>();
    static bool interactionButtonTrg = true;
    public List<MeshRenderer> cloth;
    bool love = false;//��ͳ�?
    bool marry = false;//��ȥ�߳�?
    [HideInInspector] public float friend = 0;
    public MultiAimConstraint[] aims;
    public Rig rig;
    int loverTalkNum = 0;

    public Transform[] finger;//����������� ��
    public Vector3[] gripAngle;
    public GameObject gun;
    public Rig gunRig;
    bool gripGun = false;

    bool talking = false;

    [HideInInspector]public int disputeCount = 0;
    float time = 0;

    public SpriteRenderer arrow;
    public float Friend
    {
        get => friend;
        set
        {
            friend = Mathf.Clamp(value, -100f, 100f);
        }
    }
    public void Set(string name, bool love, float friendly)
    {
        _name = name;
        this.love = love;
        Friend = friendly;
        Material full = Instantiate(materials[0].material);
        Material outline = Instantiate(materials[2].material);
        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i].material.HasProperty("_SpecularStep"))
            {
                materials[i].material = outline;
            }
            else
            {
                materials[i].material = full;
            }
        }
        Color c = Color.white;
        if (friend >= 0)
        {
            c = new Color32((byte)(255 - (Friend * 2.55f)), (byte)(255 - (Friend * 1.55f)), 255, 255);
        }
        else
        {
            c = new Color32(255, (byte)(255 - (Friend * -2.55f)), (byte)(255 - (Friend * -2.55f)), 255);
        }
        full.color = c;
        outline.color = c;
        arrow.color = c;
        inBut.GetComponent<Image>().color = c;
        if (gender)
        {
            Material full_out = Instantiate(cloth[0].material);
            cloth[0].material = full_out;
            cloth[1].material = full_out;
            full_out.color = c;
        }
    }
    private void Awake()
    {
        interactionButtonTrg = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (gameObject.CompareTag("Man"))
        {
            gender = true;
        }
        foreach (SkinnedMeshRenderer s in GetComponentsInChildren<SkinnedMeshRenderer>()) materials.Add(s);
        FindChildRecursivelyByName(transform, "mixamorig_Neck");//�� ã��
        head = neck.GetChild(0);
        inBut = Instantiate(UIManager.Instance.inBut, Vector3.zero, Quaternion.identity, UIManager.Instance.cvs.transform).transform;//��ȣ�ۿ� ��ư ����
        ProgressManager.Instance.interaction2 += OffInter;//��ư ���� �Լ��� ����
        inBut.gameObject.SetActive(false);
        inBut.GetComponent<Button>().onClick.AddListener(OnClickTalk);
    }
    private void Start()
    {
        WeightedTransformArray sources = new WeightedTransformArray();
        sources.Add(new WeightedTransform(PlayerHead.playerHead, 1f));
        aims[0].data.sourceObjects = sources;
        aims[1].data.sourceObjects = sources;
        aims[0].transform.root.GetComponent<RigBuilder>().Build();
        index = Random.Range(0, TownPointsList.Instance.length);
        navMeshAgent.SetDestination(TownPointsList.Instance.points[index].position);
    }


    private void Update()
    {
        if (navMeshAgent.isStopped == false && marry == false)
        {
            time += Time.deltaTime;
            if (time >= 10)
            {
                time = 0;
                if (Mathf.Abs(Friend) > 10)
                {
                    ButtonColor(Friend > 0 ? -10 : 10);
                }
                if (Mathf.Abs(Friend) <= 10 && love == false)
                {
                    time = -100000;
                    navMeshAgent.SetDestination(TownPointsList.Instance.exitPosition.position);
                }
            }
        }
        //navMeshAgent.SetDestination(TownPointsList.Instance.homePosition.position);
        if(transform.position.z >= 75 && gameObject.tag != "Untagged")//������ ������ ģ�� ����Ʈ���� ����
        {
            ProgressManager.Instance.interaction2 -= OffInter;
            look = false;
            gameObject.tag = "Untagged";
            Destroy(inBut.gameObject);
            EndingManager.Instance.endFriend++;
        }
        if(Vector3.Magnitude(TownPointsList.Instance.points[index].position - transform.position) < 8 && Vector3.SqrMagnitude(navMeshAgent.destination - TownPointsList.Instance.exitPosition.position) > 0.1f && Vector3.SqrMagnitude(navMeshAgent.destination - TownPointsList.Instance.homePosition.position) > 0.1f)//���� ���ƴٴϴ� ��Ȳ�̸� ���ƴٴϰ�
        {
            index = Random.Range(0, TownPointsList.Instance.length);
            navMeshAgent.SetDestination(TownPointsList.Instance.points[index].position);
        }
        bool inHome = Vector3.SqrMagnitude(navMeshAgent.destination - TownPointsList.Instance.homePosition.position) <= 0.1f && Vector3.Distance(transform.position, TownPointsList.Instance.homePosition.position) <= 0.1f;
        if (inHome)//�� ��ó�� ���� ����
        {
            if (navMeshAgent.isStopped == false)
            {
                navMeshAgent.isStopped = true;
                animator.SetBool("Trg", true);
            }
            transform.LookAt(GameManager.Instance.playerTrm, Vector3.up);
        }
        
        if (look == false && gripGun == false && talking == false)//�÷��̾ ���� �Ⱥ���������
        {
            rig.weight = Mathf.Lerp(rig.weight, 0, rotationSpeed * Time.deltaTime);//�� ������ ������
            if (inHome == false)
            {
                animator.SetBool("Trg", false);//�ȴ´�.
                navMeshAgent.isStopped = false;
            }
            if (inBut != null && inBut.gameObject.activeSelf == true)//�ٵ� ��ư�� ����������
            {
                inBut.gameObject.SetActive(false);
            }

        }
        else
        {
            LookPlayer();
        }
    }

    public void LookPlayer()
    {
        Vector3 directionToPlayer = GameManager.Instance.playerHead.position - neck.position;
        float angle = Vector3.Angle(transform.forward, new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        inBut.position = GameManager.Instance.mainCam.WorldToScreenPoint(head.position + new Vector3(0, 0.5f, 0));
        if (angle < 60)
        {
            if (inBut.gameObject.activeSelf == false && interactionButtonTrg && GameManager.Instance.talkNum > 0)
            {
                inBut.gameObject.SetActive(true);
            }
            rig.weight = Mathf.Lerp(rig.weight, 1, rotationSpeed * Time.deltaTime);
            animator.SetBool("Trg", true);
            navMeshAgent.isStopped = true;
        }
        else
        {
            if (inBut.gameObject.activeSelf == true) inBut.gameObject.SetActive(false);
            animator.SetBool("Trg", false);
            navMeshAgent.isStopped = false;
        }
    }
    public void OnClickTalk()//===========��ȭ����=============
    {
        talking = true;
        if (love)//��͸�
        {
            if(marry)//��ȥ������
            {
                ProgressManager.Instance.ProgressScenario(5, _name, ButtonColor, OnFarewall, this);//��ȥ���
            }
            else if (Friend >= 100 && loverTalkNum >= 0 && Random.value <= 0.7f && GameManager.Instance.haveWife == false)//��ʹµ� �������� ������ 2������ �ϰ� �������� ������
            {
                ProgressManager.Instance.ProgressScenario(4, _name, ButtonColor, OnFarewall, this);//ûȥ
            }
            else
            {
                ProgressManager.Instance.ProgressScenario(2, _name, ButtonColor, OnFarewall, this);//��͸� ��ж� ���
            }
        }
        else
        {
            if (!gender && Friend >= 100 && Random.value <= 0.1f && GameManager.Instance.haveLover == false && GameManager.Instance.haveWife == false)//�������� ���� ������ Ȯ���� ���
                ProgressManager.Instance.ProgressScenario(1, _name, ButtonColor, OnFarewall, this);
            else
                ProgressManager.Instance.ProgressScenario(0, _name, ButtonColor, OnFarewall, this);//�ƴϸ� ģ�����
        }
        UIManager.Instance.MapOut(1);
        PlayerController.canMove = false;
        GameManager.Instance.talkNum--;
        interactionButtonTrg = false;
        ProgressManager.Instance.OffInteraction2();
    }
    public void OffInter()
    {
        interactionButtonTrg = false;
        inBut.gameObject.SetActive(false);
    }
    public void FindChildRecursivelyByName(Transform parent, string name)//�ڽ� ������ �� ã�°�;
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                neck = child;
                return;
            }
            else
                FindChildRecursivelyByName(child, name);
        }
    }
    public float ButtonColor(float value)//�ֵ� �� �ٲ�
    {
        if (value == 1024)//��鼺��
        {
            love = true;
            GameManager.Instance.haveLover = true;
            return -1;
        }
        else if (value == 2048)
        {
            interactionButtonTrg = true;
            return -1;
        }
        else if (value == 3072)
        {
            talking = false;
            marry = true;
            GameManager.Instance.haveWife = true;
            navMeshAgent.SetDestination(TownPointsList.Instance.homePosition.position);
            return -1;
        }
        else if(value == 2000)//��ʹµ� ���� ������ result�� 2��
        {
            if(Friend >= 100)
            {
                loverTalkNum++;
            }
            return -1;
        }
        else if (value == 3000)//��ʹµ� result�� 2�Ʒ���
        {
            if(loverTalkNum > 1)
            {
                loverTalkNum = 1;
            }
            return -1;
        }
        else if(value == 4000)
        {
            disputeCount++;
            return -1;
        }
        else if (value == 5000)
        {
            disputeCount = 0;
            return -1;
        }
        else if(value <= 500)//===========��ȭ���ϼ��� �ְ� �ƴҼ��� �ִ�=============
        {
            talking = false;
            Material full = Instantiate(materials[0].material);
            Material outline = Instantiate(materials[2].material);
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].material.HasProperty("_SpecularStep"))
                {
                    materials[i].material = outline;
                }
                else
                {
                    materials[i].material = full;
                }
            }
            Friend += value;
            float lim = Mathf.Clamp(Friend, -100, 100);
            Color c = Color.white;
            if (friend >= 0)
            {
                c = new Color32((byte)(255 - (lim * 2.55f)), (byte)(255 - (lim * 1.55f)), 255, 255);
            }
            else
            {
                c = new Color32(255, (byte)(255 - (lim * -2.55f)), (byte)(255 - (lim * -2.55f)), 255);
            }
            full.DOColor(c, 2);
            outline.DOColor(c, 2).OnUpdate(() =>
            {
                arrow.color = outline.color;
                inBut.GetComponent<Image>().color = outline.color;
            });
            if (gender)
            {
                Material full_out = Instantiate(cloth[0].material);
                cloth[0].material = full_out;
                cloth[1].material = full_out;
                full_out.DOColor(c, 2);
            }
            time = 0;
            if (marry == false)
            {
                navMeshAgent.SetDestination(TownPointsList.Instance.points[index].position);
            }
            return Friend;
        }
        else
        {
            return -1;
        }
    }
    public void OnFarewall()//���� �������
    {
        ProgressManager.Instance.interaction2 -= OffInter;
        EndingManager.Instance.endFriend++;
        look = false;
        gameObject.tag = "Untagged";
        Destroy(inBut.gameObject);
        navMeshAgent.SetDestination(TownPointsList.Instance.exitPosition.position);
        navMeshAgent.isStopped = false;
        love = false;
        marry = false;
        GameManager.Instance.haveLover = false;
        GameManager.Instance.haveWife = false;
    }
    private void LateUpdate()
    {
        if (gripGun)
        {
            for (int i = 0; i < finger.Length; i++)
            {
                finger[i].localRotation = Quaternion.Euler(gripAngle[i]);
            }
        }
    }

    public void KillPlayer()
    {
        gripGun = true;
        gun.SetActive(true);
        DOTween.To(() => gunRig.weight, x => gunRig.weight = x, 1f, 1f).SetEase(Ease.Linear);
        EndingManager.Instance.EndingFunc(Ending.Murder, 0, 2);
        StartCoroutine(PlaySound());
    }
    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.Shoot();
        //�ѼҸ����
    }
}
