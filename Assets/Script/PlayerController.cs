using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float speed;
    float length = 0;
    private Rigidbody rigid;
    bool onWall = false;
    float camAngle = 0;
    public Transform mainCamera;
    public static bool canMove = true;

    public Transform[] finger;//총을잡기위한 손
    public Vector3[] gripAngle;
    public GameObject gun;
    public Rig gunRig;
    bool gripGun = false;

    float lerp = 0;

    public static bool walkSound;
    private void Awake()
    {
        canMove = true;
        walkSound = false;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        ChangeAngle();
    }

    private void ChangeAngle()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
        {
            GetCamAngle();
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
        {
            GetCamAngle();
        }
    }

    private void GetCamAngle()
    {
        /*Vector3 direction = mainCamera.position - transform.position;
        direction.Normalize();
        float aa = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg;
        camAngle = aa;*/
        camAngle = mainCamera.eulerAngles.y;
    }

    void Move()
    {
        if (onWall == false && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)) && canMove)
        {
            walkSound = true;
            EndingManager.Instance.stopTime = 0;
            Vector3 dir = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
            float a = Vector3.Magnitude(dir);
            if (a > 1)
            {
                dir.Normalize();
                a = 1;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Mathf.Abs(length - a) > 0.1f)
                {
                    length = Mathf.Min(1, length + Time.fixedDeltaTime * 2);
                }
                else
                {
                    length = a;
                }
            }
            else
            {
                a *= 0.5f;
                if (Mathf.Abs(length - a) > 0.1f)
                {
                    if (length <= 0.5f)
                    {
                        length = Mathf.Min(0.5f, length + Time.fixedDeltaTime * 2);
                    }
                    else
                    {
                        length = Mathf.Min(1, length - Time.fixedDeltaTime * 2);
                    }
                }
                else
                {
                    length = a;
                }
            }
            anim.SetFloat("Speed", length);
            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            transform.DORotate(new Vector3(0, angle + camAngle, 0), 0.3f).SetEase(Ease.Linear);
            float y = rigid.velocity.y;
            rigid.velocity = transform.TransformDirection(Vector3.forward * speed * (length <= 0.5f ? length * 2f : length * 2.2f));
            rigid.velocity = new Vector3(rigid.velocity.x, y, rigid.velocity.z);
        }
        else
        {
            length = Mathf.Max(0, length - (Time.fixedDeltaTime * 4f));
            anim.SetFloat("Speed", length);
            rigid.velocity = Vector3.zero;
            walkSound = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWall = true;
            transform.Translate(Vector3.back * 0.08f);
            Vector3 vec = transform.position;
            transform.Translate(Vector3.forward * 0.08f);
            transform.DOMove(vec, 0.7f).OnComplete(()=>
            {
                onWall = false;
            });
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "GoOut")
        {
            canMove = false;
            ProgressManager.Instance.OpeningOn();
        }
        if (other.gameObject.name == "SchoolExcape")
        {
            canMove = false;
            ProgressManager.Instance.GoTown();
        }
    }
    private void LateUpdate()
    {
        if (gripGun)
        {
            for (int i = 0; i < finger.Length; i++)
            {
                finger[i].localRotation = Quaternion.Lerp(finger[i].localRotation, Quaternion.Euler(gripAngle[i]), lerp);
            }
            lerp += Time.deltaTime;
        }
    }
    public void KillPlayer()
    {
        gripGun = true;
        gun.SetActive(true);
        DOTween.To(() => gunRig.weight, x => gunRig.weight = x, 1f, 2f).SetEase(Ease.InQuad);
        EndingManager.Instance.EndingFunc(Ending.LonelinessDie, 0, 2);
        StartCoroutine(PlaySound());
    }
    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.Shoot();
        //총소리재생
    }
}
