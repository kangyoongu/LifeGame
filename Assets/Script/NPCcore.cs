using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPCcore : MonoBehaviour, INpc
{
    readonly NameMaker namemaker = new();
    [HideInInspector]public string _name = "";
    [HideInInspector]public bool gender = false;//true���� false ����
    float rotationSpeed = 2f; // ȸ�� �ӵ��� �����մϴ�.
    [HideInInspector] public Transform neck;
    Quaternion firstQ;
    Transform head;
    Quaternion firstT;
    [HideInInspector] public bool look = false;
    float time = 0;
    private Transform inBut;//��ȣ�ۿ��ư
    [HideInInspector] public List<SkinnedMeshRenderer> materials = new List<SkinnedMeshRenderer>();
    static bool interactionButtonTrg = true;
    public List<MeshRenderer> cloth;
    [HideInInspector]public bool love = false;//��ͳ�?
    bool relation = false;//ģ������?
    [HideInInspector] public float friend = 0;
    public float Friend { 
        get => friend; 
        set
        {
            friend = Mathf.Clamp(value, -100f, 100f);
        } 
    }
    private void Awake()
    {
        interactionButtonTrg = true;
        if (gameObject.CompareTag("Man")) {
            gender = true;
            _name = namemaker.MaleName();
        }
        else
        {
            _name = namemaker.FemaleName();
        }
        foreach (SkinnedMeshRenderer s in GetComponentsInChildren<SkinnedMeshRenderer>()) materials.Add(s);
        FindChildRecursivelyByName(transform, "mixamorig_Neck");
        firstQ = neck.rotation;
        head = neck.GetChild(0);
        firstT = head.rotation;
    }
    private void Start()
    {
        interactionButtonTrg = true;
        inBut = Instantiate(UIManager.Instance.inBut, Vector3.zero, Quaternion.identity, UIManager.Instance.cvs.transform).transform;//��ȣ�ۿ� ��ư ����
        ProgressManager.Instance.interaction += OffInter;//��ư ���� �Լ��� ����
        inBut.gameObject.SetActive(false);
        inBut.GetComponent<Button>().onClick.AddListener(OnClickTalk);

        //friend = 100;//�׽�Ʈ�� �ڵ�
        //relation = true;
        //GameManager.Instance.related.Add(this);
    }
    private void Update()
    {
        if (look == false)
        {
            neck.rotation = Quaternion.Slerp(neck.rotation, firstQ, rotationSpeed * Time.deltaTime);
            head.rotation = Quaternion.Slerp(head.rotation, firstT, rotationSpeed * Time.deltaTime);
            time = 0;
            if (inBut != null && inBut.gameObject.activeSelf == true) inBut.gameObject.SetActive(false);
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
        if (angle < 68)
        {
            if (inBut.gameObject.activeSelf == false && interactionButtonTrg) inBut.gameObject.SetActive(true);
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            time += Time.deltaTime * 0.5f;
            neck.rotation = Quaternion.Slerp(firstQ, lookRotation, Mathf.Min(time, 0.5f));
            head.rotation = Quaternion.Slerp(head.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (inBut.gameObject.activeSelf == true) inBut.gameObject.SetActive(false);
        }
    }
    public void FindChildRecursivelyByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                neck = child;
                return;
            }
            else
            {
                FindChildRecursivelyByName(child, name);
            }
        }
    }
    public void OnClickTalk()//��ȭ����===============================================================
    {
        if (love)
        {
            ProgressManager.Instance.ProgressScenario(2, _name, ButtonColor, OnFarewall);//��͸� ��ж� ���
        }
        else
        {
            if (!gender && Friend >= 100 && Random.value >= 0.0f && GameManager.Instance.haveLover == false && GameManager.Instance.haveWife == false)
                ProgressManager.Instance.ProgressScenario(1, _name, ButtonColor, OnFarewall);//�ƴϸ� 10�۷� ���
            else
                ProgressManager.Instance.ProgressScenario(0, _name, ButtonColor, OnFarewall);//�ƴϸ� ģ�����
        }
        PlayerController.canMove = false;
        interactionButtonTrg = false;
        ProgressManager.Instance.OffInteraction();
    }
    public void OffInter()
    {
        interactionButtonTrg = false;
        inBut.gameObject.SetActive(false);
    }
    public float ButtonColor(float value)//value�� 1024�� love�� true�� �ƴϸ� �� �ٲ��ִ� ��Ȱ
    {
        if (value == 1024)//��鼺��
        {
            love = true;
            GameManager.Instance.haveLover = true;
            return -1;
        }
        else if(value == 2048)
        {
            interactionButtonTrg = true;
            return -1;
        }
        else if(value <= 500)
        {
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
            if (Mathf.Abs(lim) >= 30 && !relation)
            {
                relation = true;
                GameManager.Instance.related.Add(this);
            }
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
                inBut.GetComponent<Image>().color = outline.color;
            });
            if (gender)
            {
                Material full_out = Instantiate(cloth[0].material);
                cloth[0].material = full_out;
                cloth[1].material = full_out;
                full_out.DOColor(c, 2);
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
        ProgressManager.Instance.interaction -= OffInter;
        look = false;
        GameManager.Instance.haveLover = false;
        GameManager.Instance.haveWife = false; 
        gameObject.tag = "Untagged";
        Destroy(inBut.gameObject);
        GameManager.Instance.related.Remove(this);
    }
}
