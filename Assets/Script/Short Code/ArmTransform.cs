using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmTransform : MonoBehaviour
{
    TwoBoneIKConstraint rigging;
    public Transform head;
    public float transformup = -0.2f;
    public float transformright = -0.07f;
    public float transformforward = -0.07f;
    public float postprocess;
    public float gunforward = -0.2f;
    public float distance = 0.4f;
    private void Awake()
    {
        rigging = GetComponent<TwoBoneIKConstraint>();
        rigging.data.target = transform;
    }
    void Update()
    {
        Vector3 dir = GunPoint.pos.position - head.position;
        dir.Normalize();
        transform.position = head.position + (dir * distance) + transform.up * transformup + transform.forward * transformforward + transform.right * transformright;
        Vector3 targetDirection = (GunPoint.pos.position + GunPoint.pos.forward * gunforward) - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDirection);
        transform.Rotate(Vector3.right * 90);
        transform.position += transform.right * postprocess;
    }
}
